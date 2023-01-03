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
    public int teamPoints;
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

        public static LevelController instance;

        private MatchMaker matchMaker;
     
        private NetworkManager networkManager;
        
        [SyncVar]
        public Match currentMatch;

        [SyncVar]
        public string levelMatchID;

        [SyncVar]
        public bool readyToStart;

        readonly public List<Match> levelmatches = new List<Match>();
        readonly public List<Team> teams = new List<Team>();
        readonly public List<Player> matchPlayers = new List<Player>();
        readonly public List<PlayerController> gamePlayers = new List<PlayerController>();

        [SerializeField]
        GameObject playerPrefab;
        [SerializeField]
        GameObject prizePrefab;
        [SerializeField]
        GameObject teamboxPrefab;

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

    [Server]
    public void InitiateLevel(string levelMatchID)
    {
        Debug.Log("Level Controller starting for match: " + levelMatchID);
        matchMaker = GameObject.FindObjectOfType<MatchMaker>();
        networkManager = GameObject.FindObjectOfType<NetworkManager>();


        for (int i = 0; i < matchMaker.matches.Count;) {
            if (matchMaker.matches[i].matchID == levelMatchID) 
            {
                levelmatches.Add(matchMaker.matches[i]);
                Debug.Log("Passing match list from matchmaker to levelcontroller");
                i++;
            }
        }

        Debug.Log(levelmatches);


        for (int i = 0; i < matchMaker.matches.Count;)
        {   
            Debug.Log("Loop for setting up match, matchplayers and gameplayers for match ID:  " + levelmatches[i].matchID + 
            " amount of matches = " + matchMaker.matches.Count);
            if (matchMaker.matches[i].matchID == levelMatchID) 
            {currentMatch = matchMaker.matches[i];}
            matchPlayers.AddRange(currentMatch.players);
            Debug.Log("For levelMatch: " + levelMatchID +" currentMatch.matchID = " + currentMatch.matchID + " and : " 
            + matchMaker.matches[i].matchID + " what is i: " + i + " Amount of players in this match:  "+ currentMatch.players.Count);
            i++;
        }
    }
    public void CheckifLevelisReadyToStart(bool readyToStart)
    {
        if (readyToStart)  { InitiateLevel(levelMatchID);}
        else return;
    }

    public void CheckIfGamePlayersAreReady()
    {
        int k = 0; 
        foreach (Player player in matchPlayers) 
        {
            if (player.isReady == true)
            {k++;}
        }
        if (k == matchPlayers.Count)  {readyToStart = true;}
        //CheckifLevelisReadyToStart(readyToStart);
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
    //     foreach (GameObject spawnPoint in spawnPoints)  { playerSpawnPoints.Add(spawnPoint.transform);}
    //     Debug.Log("Ended getting PlayerSpawnPoints");
    // }

    //     private void GetTeamSpawnPoints(string spawnType)
    // {
    //     GameObject[] spawnPoints;
    //     spawnPoints = GameObject.FindGameObjectsWithTag(spawnType);

    //     foreach (GameObject spawnPoint in spawnPoints)   { teamSpawnPoints.Add(spawnPoint.transform);   }
    //     Debug.Log("Ended getting TeanSpawnPoints");
    // }
   

    [Server]
    public void SpawnPlayers (string levelMatchID) 
    {
        Debug.Log("SpawnPlayers function: Attempting to spawn players");
        for (int i = 0; i < matchMaker.matches.Count; i++) {       
                int t = 0;
                foreach (var player in matchPlayers) 
                {   
//                    Transform startPos = networkManager.GetStartPosition();
                    GameObject go = Instantiate(playerPrefab);
                    go.GetComponent<PlayerController>().playerIndex = player.playerIndex; // not disabling for now, if this is causing problems still can get netid of player.connectiontoServer isntead 
                    NetworkServer.ReplacePlayerForConnection(player.connectionToClient, go, true);
                    gamePlayers.Add(go.GetComponent<PlayerController>());
                    NetworkServer.SetClientReady(gamePlayers[t].connectionToClient);
                    Debug.Log("SpawnPlayers function: moved player to gamePlayer list");
                    gamePlayers[t].GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
                    t++;
                }
         
        }
        SpawnTeams();
    }

    [Server]
    public void SpawnTeams()
    {
        int playersAmount = gamePlayers.Count;
         if (playersAmount == 1 || playersAmount == 5 || playersAmount == 7)
         {
            for ( int i = 0; i <playersAmount; )
            {
                Team team = new Team (i.ToString(), gamePlayers[i]);
                team.teamID = i.ToString();
                teams.Add(team);
                i++;
            }
         }

         if (playersAmount == 2 || playersAmount ==4 || playersAmount == 8)
         {
            for ( int i = 0; i <playersAmount; )
            {
                Team team = new Team (i.ToString(), gamePlayers[i]);
                team.teamID = i.ToString();
                teams.Add(team);

                foreach (var player in gamePlayers)
                {
                    team.players.Add(gamePlayers[i]);
                }
                i++;
            }
         }
        SpawnTeamboxes();
        SetClientsReady();
    }

    [Server]
    public void SpawnTeamboxes() 
    {
        for (int i = 0; i < teams.Count;)
        {
            Debug.Log("Spawn Teamboxes function: Spawning teamboxes for: " +teams.Count + " teams");
            GameObject go = Instantiate(teamboxPrefab);
            NetworkServer.Spawn(go);     
            i++ ; 
        }

        Debug.Log("PrepareLevel function: Preparing for making clients ready");
        MovePlayersToTeams();
        
    }

// ### Seems to be overcomplicated or not in right script. 
// Variable assigned to player>>gameplayer of int teamid should be more than enough
// Team assignement may be done in matchmaker by players on their own depending on a game mode.

    [Server]
    public void MovePlayersToTeams()
    {
        if (teams.Count == 2)
        {
            foreach (var player in gamePlayers)
            {
                for (int i = 0; i < gamePlayers.Count; )
                {
                    if (IsOdd(i))   { teams[0].players.Add(gamePlayers[i]);}
                    if (!IsOdd(i))  { teams[1].players.Add(gamePlayers[i]);}
                    i++;
                }
            }
        }
    }

    public static bool IsOdd(int value)
    {
        return value % 2 != 0;
    }

// ## Have to be finished after figuring out how to pass transform of level 
    public void SetClientsReady()
    {
        int index = 0;
        foreach (var playerController in gamePlayers)
        {
            gamePlayers[index].SetPlayerReady(false,true);
           // gamePlayers[index].gameObject.transform.SetPositionAndRotation(gamePlayers[index].transform.position, gamePlayers[index].transform.rotation);
            index++;
            NetworkServer.SetClientReady(playerController.connectionToClient);
        }
        Debug.Log("Clients set to ready from server");
    }
    
    [TargetRpc]
    public void EndLevel()
    {
        if (!gameEnded) {return;}
        SceneManager.UnloadSceneAsync("OnlineScene");
    }
    public bool CompareMatchId ()
    {
        if (this.currentMatch.matchID == NetworkClient.connection.identity.GetComponent<Player>().matchID)
        { return true;}
        else return false;
    }
}


}


