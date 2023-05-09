using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Collections.Generic;
using UnityEngine.UI;


namespace MirrorBasics{
public class UIGameplay : MonoBehaviour
{
    public static UIGameplay uiGameplay;
    public UIScore uiScore;
    public PlayerController player; 
    public NetworkRoomPlayer lobbyPlayer;
    private LobbySystem lobbySystem;
    public LevelController levelController;

    public Text playerName;

    [SerializeField] public Canvas preGameUICanvas;   
    [SerializeField] public Canvas gameUICanvas;
    [SerializeField] public Canvas postGameUICanvas;
    [SerializeField] public List<Canvas> uiStates;
    [SerializeField] public Transform ScoreboardTransform;
    public Image interactImage;

    public int uiState = 0;

    public void Start ()
    {
        lobbySystem = FindObjectOfType<LobbySystem>();
        ChangeUIState(0);             
    }

    public void ChangeUIState (int uiState)
    {
        int i = uiState;
        foreach (Canvas canvas in uiStates)
        { 
            canvas.enabled = false;
            if (uiState == i) {uiStates[i].enabled = true;}  
        }
        Debug.Log("Changing UI state to: " + i);
    }

    public void ImReady()
    {
        lobbyPlayer.readyToBegin = true;
        ChangeUIState(1);
    }

    public void SetGameplayerStateReady()
    {
        player.SetReadyState(false, true);
        Debug.Log("My Gameplayer is setting up to be ready, passing info to the player controller to call it at level controller");
    }

    public void StartLevel()
    {
        levelController.InitiateLevel();
    }

    public void LoadRoomScene()
    {
        NetworkRoomManagerExt.singleton.ServerChangeScene(NetworkRoomManagerExt.singleton.RoomScene);
    }

    public void QuitLevel()
    {    
        levelController.QuitLevel();
    }

    public void ReturnToLobby()
    {
        lobbySystem.lobbyPanel.gameObject.SetActive(true);
        lobbySystem.OpenLobbyMenu();
        NetworkClient.Disconnect();
        NetworkServer.Shutdown();   
        SceneManager.LoadScene("Empty"); 
    }

    public void BackToLobby()
    {
        FindObjectOfType<LobbySystem>().gameObject.SetActive(true);
        FindObjectOfType<LobbySystem>().OpenLobbyMenu();
        NetworkClient.Disconnect();    

    }

    public void DisplayScoreboardPrefabs()
    {

    }

}
}

