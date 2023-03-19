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
    public LevelController levelController;

    [SerializeField] public Canvas preGameUICanvas;   
    [SerializeField] public Canvas gameUICanvas;
    [SerializeField] public Canvas postGameUICanvas;
    [SerializeField]public List<Canvas> uiStates;
    public Image interactImage;

    public int uiState = 0;

    public void Start ()
    {
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

// Function logic can be moved to lobby player instead to get it only for callout here
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
        NetworkRoomManagerExt.singleton.ServerChangeScene("RoomScene");
    }

    public void QuitLevel()
    {    
        levelController.QuitLevel();
    }

}
}

