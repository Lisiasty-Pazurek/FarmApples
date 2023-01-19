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
//    public int teamPoints;
//    private int requiredScore = 10;
    public List<PlayerController> players = new List<PlayerController> ();

    public Team (string teamID, PlayerController playerCtrl)
    {
        this.teamID = teamID;
        players.Add(playerCtrl);
    }

    public Team () {}
    }

public class LevelController : NetworkBehaviour
{   

//        public static LevelController instance;

        private MatchMaker matchMaker;
     
        private NetworkManager networkManager;
        
        [SyncVar] public Match currentMatch;

        [SyncVar] public string levelMatchID;

        [SyncVar] public bool readyToStart;

        public bool readyToStartLevel;

        readonly public List<Match> levelmatches = new List<Match>();
        readonly public List<Team> teams = new List<Team>();
        readonly public List<Player> matchPlayers = new List<Player>();
        readonly public List<PlayerController> gamePlayers = new List<PlayerController>();

        readonly public List<GameObject> spawnedItems = new List<GameObject>();

        [SerializeField] GameObject playerPrefab;
        [SerializeField] GameObject playerPrefabSheep;
        [SerializeField] GameObject playerPrefabDonkey;
        [SerializeField] GameObject prizePrefab;
        [SerializeField] GameObject teamboxPrefab;

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
        }

        public override void OnStartLocalPlayer()   { }

        public override void OnStartServer()  { }

    [Server]
    public void InitiateLevel(string levelMatchID)
    {
        Debug.Log("Level Controller starting for match: " + levelMatchID);
        matchMaker = GameObject.FindObjectOfType<MatchMaker>();
        networkManager = GameObject.FindObjectOfType<NetworkManager>();

        for (int i = 0; i < matchMaker.matches.Count; i++) {
            if (matchMaker.matches[i].matchID == levelMatchID) 
            {
                levelmatches.Add(matchMaker.matches[i]);
                Debug.Log("Passing match list from matchmaker to levelcontroller");
            }
        }
        for (int i = 0; i < matchMaker.matches.Count;i++)
        {   
            if (matchMaker.matches[i].matchID == levelMatchID) 
            {
                currentMatch = matchMaker.matches[i];
                matchPlayers.AddRange(currentMatch.players);
                Debug.Log("For levelMatch: " + levelMatchID +" currentMatch.matchID = " + currentMatch.matchID + " and : " + matchMaker.matches[i].matchID + " what is i: " + i + " Amount of players in this match:  "+ currentMatch.players.Count);
            }
            
        }
        CheckIfMatchPlayersAreReady();
    }

    public void CheckIfMatchPlayersAreReady()
    {
        int k = 0; 
        foreach (Player player in matchPlayers) 
        {
            if (player.isReady == true)
            {k++;}
        }
        if (k == matchPlayers.Count)  {readyToStart = true;}
        // CheckifLevelisReadyToStart(readyToStart);
        PrepareLevel(levelMatchID);
    }


    [Server]
    public void PrepareLevel(string levelMatchID)
    {
        int playersAmount = matchPlayers.Count;
        Debug.Log("Players in game: " + playersAmount);
        // GetPlayerSpawnPoints("PlayerSpawnPoint");
        // GetTeamSpawnPoints("TeamSpawnPoint");
        SpawnPlayers(levelMatchID);
    }

// ### Disabled for debugging + setting up/moving to other script as a server doesnt have access to loaded level 

    // private void GetPlayerSpawnPoints(string spawnType)
    // {
    //     GameObject[] spawnPoints;
    //     spawnPoints = GameObject.FindGameObjectsWithTag(spawnType);
    //     foreach (GameObject spawnPoint in spawnPoints)  
    //     { playerSpawnPoints.Add(spawnPoint.transform);}
    //     Debug.Log("Ended getting PlayerSpawnPoints");
    // }

    //     private void GetTeamSpawnPoints(string spawnType)
    // {
    //     GameObject[] spawnPoints;
    //     spawnPoints = GameObject.FindGameObjectsWithTag(spawnType);
    //     foreach (GameObject spawnPoint in spawnPoints)   
    //     { teamSpawnPoints.Add(spawnPoint.transform);}
    //     Debug.Log("Ended getting TeanSpawnPoints");
    // }
   

    [Server]
    public void SpawnPlayers (string levelMatchID) 
    {
        Debug.Log("SpawnPlayers function: Attempting to spawn players");
           
                int t = 0;
                int t2 = 0; 
                
                foreach (var player in matchPlayers) 
                {   

                    if (player.matchID != levelMatchID) {return;} 
                    if (IsOdd(t)) {playerPrefab = playerPrefabDonkey; t2 =45; } 
                    else {playerPrefab = playerPrefabSheep; t2 =0;}

                    Vector3 startPos = new Vector3(4 +t*2, 0, t2);
                    GameObject go = Instantiate(playerPrefab, startPos, Quaternion.identity);
                    if (!IsOdd(matchPlayers.Count)&&IsOdd(t)) {go.GetComponent<PlayerController>().moveSpeed=6;} 
                    else {go.GetComponent<PlayerController>().moveSpeed=5;};

                    go.GetComponent<PlayerController>().playerIndex = player.playerIndex; 
                    go.GetComponent<NetworkMatch>().matchId = player.GetComponent<NetworkMatch>().matchId;
                    if (IsOdd(t)) {go.GetComponent<PlayerScore>().teamID = 2;}

                    NetworkServer.ReplacePlayerForConnection(player.connectionToClient, go, true);
                    gamePlayers.Add(go.GetComponent<PlayerController>());
                    NetworkServer.SetClientReady(gamePlayers[t].connectionToClient);

                    Debug.Log("SpawnPlayers function: moved player to gamePlayer list");
                    gamePlayers[t].GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;

                    spawnedItems.Add(go);
                    t++;
                 }
        SpawnTeamboxes();
    }


    [Server]
    public void SpawnTeamboxes() 
    {
        int t = 0;
        for (int i = 0; i < 2; i++)
        {
            Vector3 spawnPosition = new Vector3( 0, 0, t*40);
            Debug.Log("Spawn Teamboxes function: Spawning teamboxes for: " +teams.Count + " teams");
            GameObject go = Instantiate(teamboxPrefab, spawnPosition, Quaternion.identity);
            go.GetComponent<NetworkMatch>().matchId = this.currentMatch.matchID.ToGuid();
            go.GetComponent<TeamBox>().teamID = t+1;
            NetworkServer.Spawn(go);
            spawnedItems.Add(go);     
            t ++;
        }
        Debug.Log("PrepareLevel function: Preparing for making clients ready");       
    }

    public static bool IsOdd(int value)
    {
        return value % 2 != 0;
    }

[Command (requiresAuthority = false)]
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

        if (readyToStartLevel) {
            foreach (PlayerController gamePlayer in gamePlayers) 
            {
                gamePlayer.SetPlayerReady(false, true);
                Debug.Log("Final setting levelcontroller to ready gamePlayerof id: " +gamePlayer.netId );
            }
        }
    }

    [ClientRpc]
    public void EndLevel()
    {
        ClientLeaveMatch();
        CleanSpawnedObjects();
    }

   
    private void ClientLeaveMatch() 
    {
        Player.localPlayer.UnloadClientScene("OnlineScene");
        Player.localPlayer.uIGameplay.ChangeUIState(0);        
    }

[Server]
    public void CleanSpawnedObjects()
    {
        foreach (GameObject item in spawnedItems)
        {
            Destroy(item);
        }
    }
    

    public bool CompareMatchId ()
    {
        if (this.currentMatch.matchID == NetworkClient.connection.identity.GetComponent<Player>().matchID)
        { return true;}
        else return false;
    }
}


}


