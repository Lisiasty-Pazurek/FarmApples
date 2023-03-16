using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using MirrorBasics;

public class UI_Room : NetworkBehaviour
{
    public LobbySystem lobbySystem;
    public GameObject roomPlayer;
    public NetworkRoomPlayerExt roomPlayerScript;
    public override void OnStartClient() 
    {
        lobbySystem = FindObjectOfType<LobbySystem>();
        //roomPlayer = localPlayer.gameObject;
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

        if (isClientOnly)
        {
            Debug.Log("ree " + NetworkClient.localPlayer.gameObject.GetComponent<NetworkRoomPlayerExt>().index);
            NetworkClient.localPlayer.gameObject.GetComponent<NetworkRoomPlayerExt>().CmdChangeReadyState(true);
        }

        if (isServer)
        {
            roomPlayer = NetworkClient.localPlayer.gameObject;
            roomPlayerScript = roomPlayer.GetComponent<NetworkRoomPlayerExt>();
//            roomPlayerScript.CmdChangeReadyState(true);
            //Debug.Log("ree " + roomPlayerScript.index);

//            NetworkClient.connection.identity.gameObject.GetComponent<NetworkRoomPlayerExt>().CmdChangeReadyState(state);
        }
    }

}
