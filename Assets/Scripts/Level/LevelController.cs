using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MirrorBasics {
   
    [System.Serializable]
    public class Team 
    {
    [SyncVar]
    public string teamID;
    [SyncVar]
    public List<PlayerController> players = new List<PlayerController> ();

     }

public class LevelController : NetworkBehaviour
{   

    private NetworkRoomManagerExt networkManager;
        

    [SyncVar] public bool readyToStart;

    public GameMode gameMode; 

    public bool readyToStartLevel;
    [SerializeField] private float countdownDuration = 1f;


        readonly public List<NetworkRoomPlayer> matchPlayers = new List<NetworkRoomPlayer>();
        readonly public List<PlayerController> gamePlayers = new List<PlayerController>();

        readonly public List<GameObject> spawnedItems = new List<GameObject>();

        [SerializeField] GameObject playerPrefab;
        [SerializeField] GameObject playerPrefabSheep;
        [SerializeField] GameObject playerPrefabDonkey;
        [SerializeField] GameObject prizePrefab;
        [SerializeField] GameObject teamboxPrefab;
        [SerializeField] GameObject teamboxPrefab2;

        readonly public List<Transform> playerSpawnPoints = new List<Transform> ();
        private ArrayList spawnPoints;

        // [SerializeField]
        // public Transform teamSpawnPoint;
      
        readonly public List<Transform> teamSpawnPoints = new List<Transform> ();

        public bool gameEnded = false;

        private Scene onlineScene;
        private PlayerController pController;

        public void Start() {  }

        public override void OnStartClient() 
        {
            UIGameplay uIGameplay = FindObjectOfType<UIGameplay>();
            // if (!CompareMatchId()) {return;} // it will be necessary for multiple spawned levels on server
            // else 
            uIGameplay.levelController = this;
            gameMode = this.GetComponent<GameMode>();
        }

        public override void OnStartLocalPlayer()   
        { 

        }

        public override void OnStartServer() 
        {
            gameMode = this.GetComponent<GameMode>();
        }

    [Server]
    public void InitiateLevel()
    {
        CheckIfMatchPlayersAreReady();
    }

    public void CheckIfMatchPlayersAreReady()
    {
        if (readyToStart){return;}
        int k = 0; 
        foreach (PlayerController player in gamePlayers) 
        {
            if (player.isReady == true)
            {k++;}
        }
        if (k == gamePlayers.Count)  {readyToStart = true;}

        PrepareLevel();
       
    }


    [Server]
    public void PrepareLevel()
    {
        int playersAmount = matchPlayers.Count;
        Debug.Log("Players in game: " + playersAmount);

        SpawnPlayers();
    }

   

    [Server]
    public void SpawnPlayers () 
    {
        Debug.Log("SpawnPlayers function: Attempting to spawn players");
        SpawnTeamboxes();
    }


    [Server]
    public void SpawnTeamboxes() 
    {
        Debug.Log("PrepareLevel function: Preparing for making clients ready");       
    }

    [Server]
    public void SetTeamBox(GameObject go)
    {

    }

    public static bool IsOdd(int value)
    {
        return value % 2 != 0;
    }

[Server]
    public void CheckIfGamePlayersAreReady()
    {
        int k = 0; 
        foreach (PlayerController gamePlayer in gamePlayers) 
        {
            if (gamePlayer.isReady == true)
            k++;
            Debug.Log("checking if gameplayer ready: " + gamePlayer.netId + " is ready: " + gamePlayer.isReady);
        }

        if (k == gamePlayers.Count)  {readyToStartLevel = true;}
        Debug.Log(" [2] gamePlayers amount: " + gamePlayers.Count + " loop of: " + k + " is game ready to start? " + readyToStartLevel);

        if (!readyToStartLevel){ return;} 
        else { StartCoroutine(Countdown());  }
    }

    [Server]
    private IEnumerator Countdown()
    {
        float timeLeft = countdownDuration;
        while (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            Debug.Log(" Countdown for " + timeLeft);
            yield return new WaitForSeconds(.1f);
        }
        Debug.Log("Ending Countdown  " );
        SetGamePlayersReady();
    }

    [Server]
    private void SetGamePlayersReady()
    {

        foreach (PlayerController gamePlayer in gamePlayers) 
            {
                gamePlayer.SetPlayerReady(false, true);
                Debug.Log("Final setting levelcontroller to ready gamePlayerof id: " +gamePlayer.netId );
            }
    }

    [ClientRpc]
    public void EndLevel()
    {
        Debug.Log("Ending level for match: " );
        
    }




    

}


}


