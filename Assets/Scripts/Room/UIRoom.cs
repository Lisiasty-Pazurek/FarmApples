using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using MirrorBasics;

public class UIRoom : NetworkBehaviour
{
    public LobbySystem lobbySystem;
//    public GameObject roomPlayerObject;
    public NetworkRoomPlayerExt roomPlayer;
    public NetworkRoomManagerExt roomManager;
    public Button startbutton;
    [SerializeField] public Transform location;

    public void  Start() {
        
    }

    public override void OnStartLocalPlayer () 
    {
        lobbySystem = FindObjectOfType<LobbySystem>();
//        roomPlayerObject = NetworkClient.localPlayer.gameObject;
        //Debug.Log("ree" + NetworkRoomPlayerExt.localPlayer.index);
    }

    public override void OnStartServer ()
    {
        lobbySystem = FindObjectOfType<LobbySystem>();
    }
    

    public void BackToLobby()
    {
        lobbySystem.OpenLobbyMenu();
    }

    public void ChangeReadyState()
    {
        {
            roomPlayer.CmdChangeReadyState(true);
            //Debug.Log("ree " + roomPlayerScript.index);

//            NetworkClient.connection.identity.gameObject.GetComponent<NetworkRoomPlayerExt>().CmdChangeReadyState(state);
        }
    }

    public void ShowStartButton (bool state)
    {
        startbutton.enabled = state;
    }

 
    public void StartGame ()
    {
        roomManager.ServerChangeScene("GameplayScene");
    }


}
