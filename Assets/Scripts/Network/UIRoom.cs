using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MirrorBasics;
using UnityEngine.EventSystems;

public class UIRoom : MonoBehaviour
{
    public LobbySystem lobbySystem;
    public NetworkRoomPlayerExt roomPlayer;
    public NetworkRoomManagerExt roomManager;
    public Text readybutton;
    public GameObject startbutton;
    

    [SerializeField] public Transform location;

    public void Start() 
    {
        lobbySystem = FindObjectOfType<LobbySystem>();
        roomManager = FindObjectOfType<NetworkRoomManagerExt>();    
    }

    public void BackToLobby()
    {
        lobbySystem.OpenLobbyMenu();
    }

    public void ChangeReadyState()
    {
        roomPlayer.CmdChangeReadyState(!roomPlayer.readyToBegin);

    }

    public void ShowStartButton (bool state)
    {
        // that's cheating but it changes state at same moment as it shows button
        Debug.Log("changing button to:  " + state);
        startbutton.SetActive(!state);
    }

 
    public void StartGame ()
    {
        if (!roomManager.allPlayersReady) {return;}
        roomManager.ServerChangeScene(roomManager.GameplayScene);
    }


}
