using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;


namespace MirrorBasics{
public class UIGameplay : MonoBehaviour
{

    [SerializeField]
    public PlayerController player; 
    public Player lobbyPlayer;
    public UILobby uiLobby;

    public LevelController levelController;

    [SerializeField]
    public Canvas gameUICanvas;
    
    public void Start ()
    {
        uiLobby = GameObject.FindObjectOfType<UILobby>();        
        
        // lobbyPlayer = NetworkClient.connection.identity.GetComponent<Player>();
        // CheckReferences();
    }

// Function logic can be moved to lobby player instead to get it only for callout here
    public void ImReady()
    {
        // CheckReferences();
        // lobbyPlayer = NetworkClient.connection.identity.GetComponent<Player>();
        // Debug.Log("Tried to get my lobbyPlayer of: " + lobbyPlayer);
        lobbyPlayer.playerReady(false, true);
        uiLobby.lobbyUICanvas.enabled = false;
//        CmdCheckIfLobbyPlayersAreReady();
    }

// Unused now, should be moved to level manager instead, I'd rework it for some additional debug.log
    public void CheckIfLobbyPlayersAreReady()
    {
        levelController.CheckIfGamePlayersAreReady();

    }


// Simple debugging command. I will keep it for now
    public void IsMyCLientActive()
    {
        Debug.Log(" Is my client active?" + NetworkClient.active);
        Debug.Log(" Is my server active?" + NetworkServer.active);
    }

    public void SetPlayerReady()
    {
//        CheckReferences();
        if (player != null) {return;}
        else 
        {
            player = NetworkClient.connection.identity.GetComponent<PlayerController>(); 
        }
        player.SetPlayerReady(false, true);
    }

    public void Update() {    }

    public void CheckReferences()
    {
            if (uiLobby != null) {return;}
            else {uiLobby = GameObject.FindObjectOfType<UILobby>();}

            if (player != null) {return;}
            else {player = NetworkClient.connection.identity.GetComponent<PlayerController>(); }

            if (lobbyPlayer != null) {return;}
            else {lobbyPlayer = NetworkClient.connection.identity.GetComponent<Player>();}

            if (levelController != null) {return;}
            else {levelController = GameObject.FindObjectOfType<LevelController>();}
    }



    public void StartLevel()
    {
        string matchID = lobbyPlayer.currentMatch.matchID;
        levelController.InitiateLevel(matchID);
    }

    public void CheckifLevelReady()
    {
        lobbyPlayer.CheckLevelReady();
    }
    public void QuitLevel()
    {
        SceneManager.UnloadSceneAsync("OnlineScene");
    }

// not needed for now if ever at all
    // public void SetMyPlayerActiveScene ()
    // {
    //     player.SetClientActiveGameplayScene();
    // }

}
}

