using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

namespace MirrorBasics {

public class LevelController : NetworkBehaviour
{   
    [Header ("Attributes")]
        private NetworkRoomManagerExt networkManager;
        [SerializeField]public GameMode gameMode;     
        [SerializeField]public Spawner spawner;    
        [SyncVar] public bool readyToStart;
        public bool readyToStartLevel;
        [SyncVar] public bool gameEnded = false;    
        public bool gameFinished = false;
        public int requiredScore;
        [SyncVar] public float countdownTimer;  
        [SyncVar] public float gameTimer;

    [Header ("References")]
        [SerializeField] GameObject playerPrefab;
        [SerializeField] private Text countdownText;
        [SerializeField] private Canvas rulesCanvas;
        [SerializeField] private UIGameplay uIGameplay;
        public PlayerController pController;
        public GameObject FinalScoreboardRowPrefab;

    [Header ("Lists")]
        public List<PlayerController> gamePlayers = new List<PlayerController>();
        public List<GameObject> spawnedItems = new List<GameObject>();

        public readonly SyncDictionary<string, float> scoreboardDictionary = new SyncDictionary<string, float>();

        public void Start() {  }

        public override void OnStartClient() 
        {
            uIGameplay = FindObjectOfType<UIGameplay>();
            uIGameplay.levelController = this;
            gameMode = this.GetComponent<GameMode>();
        }

        public override void OnStartLocalPlayer()   
        { 

        }

        public override void OnStartServer() 
        {
            gameMode = this.GetComponent<GameMode>();
            InitiateLevel();
        }

    [ServerCallback]
    public void Update() 
    {
        
        gameTimer += Time.deltaTime;
    }

    [Server]
    public void InitiateLevel()
    {
        CheckIfMatchPlayersAreReady();
    }

    public bool CheckIfMatchPlayersAreReady()
    {
        
        int k = 0; 
        foreach (PlayerController player in gamePlayers) 
        {
            if (player.isReady == true)
            {k++;}
        }
        if (k == gamePlayers.Count)  {readyToStart = true;}

        PrepareLevel();
           
        return readyToStart;
    }



    [Server]
    public void PrepareLevel()
    {
        int playersAmount = gamePlayers.Count;
        Debug.Log("Players in game: " + playersAmount);
        PreparePlayers();
        if (SceneManager.GetActiveScene().name == "Farmaze")
        {
            int id = Random.Range(0, gameMode.levelPrefab.Count());
            Debug.Log("setting prefab level pre rpc");
            GameObject level = Instantiate(gameMode.levelPrefab[id]);
            NetworkServer.Spawn(level);
        }
        StartCoroutine(Countdown());     
    }



    [Server]
    public void PreparePlayers () 
    {
        Debug.Log("SpawnPlayers function: Attempting to spawn players");
        if (gameMode.gameModeName == "Farmapples")
        {
            SpawnTeamboxes();
        }
    }


    [Server]
    public void SpawnTeamboxes() 
    {
        spawner.SpawnTeamboxes();
    }

    [Server]
    public void SetTeamBox(GameObject go)   {  }



    [Server]
    private IEnumerator Countdown()
    {

        float timeLeft = countdownTimer;
        while (countdownTimer >= 0)
        {
            yield return new WaitForSecondsRealtime(1);
            int secondsLeft = Mathf.CeilToInt(countdownTimer);
            RpcUpdateCountdown(secondsLeft);
            Debug.Log(" Countdown for " + timeLeft);
            countdownTimer-= 1;
            if (countdownTimer == 2)
            {
               SetPlayerModels();   
            }
        }
        if (countdownTimer < 0)
        {
            Debug.Log("Ending Countdown  ");
            SetGamePlayersReady();
            RpcDisableCountdown();
        }
    }

    [ClientRpc]
    private void RpcUpdateCountdown(int secondsLeft)
    {
        countdownText.text = secondsLeft.ToString();
    }
    [ClientRpc]
    private void RpcDisableCountdown()
    {
        //countdownText.enabled = false;
        rulesCanvas.enabled = false;
    }


    [Server]
    private void SetPlayerModels()
    {
        foreach (PlayerController gamePlayer in gamePlayers) 
        {
            gamePlayer.SetModel();
        }
    }

    [Server]
    public void SetGamePlayersReady()
    {
        foreach (PlayerController gamePlayer in gamePlayers) 
        {
            gamePlayer.SetPlayerReady(false,true);
            Debug.Log("Final setting levelcontroller to ready gamePlayerof id: " +gamePlayer.netId );
        }
    }

    
    [Command  (requiresAuthority = false)]
    public void CheckifPlayersFinished()
    {   
        if (gameEnded && !gameFinished)
        {
            EndLevel();   

            gameFinished = true;  
        }

        int k = 0;        
        for (int i = 0; i < gamePlayers.Count; i++)
        {   
            if (gamePlayers[i].gameObject.GetComponent<Runner>().visitedCheckpoints.ContainsKey(requiredScore))
            {
                Debug.Log("Player visited last checkpoint " + k + "of: " + gamePlayers[i].gameObject);
                k += 1;
            }
            if (k > gamePlayers.Count -1 && !gameEnded)
            {
                gameEnded = true;
                MakeScoreboardDictionary();                    
            }
            Debug.Log("Ending Race?  " + k + "of: " + gamePlayers.Count);
        }
    }

    [Server]
    public void MakeScoreboardDictionary()
    {
        foreach (PlayerController player in gamePlayers)
        {
            scoreboardDictionary[player.playerName] = player.GetComponent<Runner>().visitedCheckpoints[requiredScore];         
        }
        //scoreboardDictionary.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
    }


    [ClientRpc]
    public void EndLevel()
    {
        Debug.Log("Ending level for match: " );
        uIGameplay.ChangeUIState(2);
        pController.characterController.GetComponent<CharacterController>().enabled = false;
        if (gameMode.gameModeName == "Farmarathon" )
        {   
            DisplayScoreboardPrefabs();
        }
    }

    [Client]
    public void DisplayScoreboardPrefabs()
    {
        Debug.Log("Spawning scores " );

        foreach (KeyValuePair<string,float> entry in scoreboardDictionary)
        {
            GameObject finalScoreRowObject = Instantiate(FinalScoreboardRowPrefab);
            finalScoreRowObject.GetComponent<FinalScoreRow>().playerName.text = entry.Key;
            finalScoreRowObject.GetComponent<FinalScoreRow>().playerTime.text = entry.Value.ToString();
            finalScoreRowObject.transform.SetParent(uIGameplay.ScoreboardTransform);
            Debug.Log("Spawned score prefab for: " + entry);  
        }   

    }


    public void QuitLevel()
    {
        if (isServer)
        {
            NetworkRoomManagerExt.singleton.ServerChangeScene(NetworkRoomManagerExt.singleton.RoomScene);
        }

        if (isClientOnly)
        {
            NetworkClient.Disconnect();         
            LobbySystem.singleton.OpenLobbyMenu();
        }
    }

}


}


