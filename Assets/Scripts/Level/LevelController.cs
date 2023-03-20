using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;

namespace MirrorBasics {
   
    [System.Serializable]
    public class Team 
    {
        [SyncVar] public string teamID;
        [SyncVar] public List<PlayerController> players = new List<PlayerController> ();
    }

public class LevelController : NetworkBehaviour
{   
    [Header ("Attributes")]
        private NetworkRoomManagerExt networkManager;
        [SerializeField]public GameMode gameMode;     
        [SerializeField]public Spawner spawner;    
        [SyncVar] public bool readyToStart;
        public bool readyToStartLevel;
        public bool gameEnded = false;    
        [SyncVar] public float countdownTimer;  


    [Header ("References")]
        [SerializeField] GameObject playerPrefab;
        [SerializeField] private Text countdownText;
        [SerializeField] private Canvas rulesCanvas;
        [SerializeField] private UIGameplay uIGameplay;
        public PlayerController pController;

    [Header ("Lists")]
        public List<PlayerController> gamePlayers = new List<PlayerController>();
        public List<GameObject> spawnedItems = new List<GameObject>();

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
        StartCoroutine(Countdown());     
    }

    [Server]
    public void PreparePlayers () 
    {
        Debug.Log("SpawnPlayers function: Attempting to spawn players");
        SpawnTeamboxes();
    }


    [Server]
    public void SpawnTeamboxes() 
    {
        spawner.SpawnTeamboxes();
    }

    [Server]
    public void SetTeamBox(GameObject go)
    {

    }



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

    [ClientRpc]
    public void EndLevel()
    {
        Debug.Log("Ending level for match: " );
        uIGameplay.ChangeUIState(2);
        pController.characterController.GetComponent<CharacterController>().enabled = false;
    }

    public void QuitLevel()
    {
        if (isServer)
        {
            NetworkRoomManagerExt.singleton.ServerChangeScene(NetworkRoomManagerExt.singleton.RoomScene);
        }

        if (isClientOnly)
        {
            //NetworkClient.Disconnect();
            //SceneManager.LoadSceneAsync("LobbySample");
            //LobbySystem.singleton.OpenLobbyMenu();
        }
    }

}


}


