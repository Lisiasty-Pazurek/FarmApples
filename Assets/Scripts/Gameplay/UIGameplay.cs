using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using System.Collections.Generic;

namespace MirrorBasics{
public class UIGameplay : MonoBehaviour
{
    public static UIGameplay uiGameplay;
    public PlayerController player; 
    public Player lobbyPlayer;
    public LevelController levelController;

    [SerializeField] public Canvas uiLobby;
    [SerializeField] public Canvas preGameUICanvas;   
    [SerializeField] public Canvas gameUICanvas;
    [SerializeField] public Canvas postGameUICanvas;

    [SerializeField]public List<Canvas> uiStates;

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
        lobbyPlayer.playerReady(false, true);
        ChangeUIState(1);
    }

// Simple debugging command. I will keep it for now
    public void IsMyCLientActive()
    {
        Debug.Log(" Is my client active?" + NetworkClient.active);
        Debug.Log(" Is my server active?" + NetworkServer.active);
    }

    public void SetPlayerReady()
    {
        player.SetPlayerReady(false, true);
        ChangeUIState(2);
    }

    public void SetGameplayerStateReady()
    {
        player.SetReadyState(false, true);
        levelController.CheckIfGamePlayersAreReady();
        Debug.Log("My Gameplayer is setting up to be ready, passing info to the player controller to call it at level controller");
    }

    public void Update() {    }

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
        levelController.currentMatch.players.Remove(NetworkClient.connection.identity.GetComponent<Player>());
    }

}
}

