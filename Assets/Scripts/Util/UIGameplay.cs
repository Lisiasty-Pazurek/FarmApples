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
        lobbyPlayer = NetworkClient.connection.identity.GetComponent<Player>();
        uiLobby = GameObject.FindObjectOfType<UILobby>();
        CheckReferences();
    }

    public void ImReady()
    {
        CheckReferences();
        lobbyPlayer = NetworkClient.connection.identity.GetComponent<Player>();
        Debug.Log("Tried to get my lobbyPlayer of: " + lobbyPlayer);
        lobbyPlayer.playerReady(false, true);
        uiLobby.lobbyUICanvas.enabled = false;
//        CmdCheckIfLobbyPlayersAreReady();
    }




    public void CmdCheckIfLobbyPlayersAreReady()
    {
        levelController.CheckIfGamePlayersAreReady();
    }

   



    public void IsMyCLientActive()
    {
        Debug.Log(" Is my client active?" + NetworkClient.active);
        Debug.Log(" Is my server active?" + NetworkServer.active);
    }

    public void SetPlayerReady()
    {
        CheckReferences();
        if (player != null) {return;}
        else 
        {
            player = NetworkClient.connection.identity.GetComponent<PlayerController>(); 
        }
        player.SetPlayerReady(false, true);
    }

    public void FixedUpdate() {
        //CheckReferences();
    }

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

    public void QuitLevel()
    {
        SceneManager.UnloadSceneAsync("OnlineScene");
    }

    public void SetMyPlayerActiveScene ()
    {
        player.SetClientActiveGameplayScene();
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

}
}

