using UnityEngine;
using UnityEngine.UI;
using MirrorBasics;
using Mirror;
using System.Collections.Generic;

public class UIRoom : MonoBehaviour
{
    public static UIRoom singleton {get; private set;}
    public LobbySystem lobbySystem;
    public NetworkRoomPlayerExt roomPlayer;
    public NetworkRoomManagerExt roomManager;
    public Text readybutton;
    public GameObject startbutton;
    public GameObject switchRoleButton;
    public Dropdown modelName;
    
    public List<Transform> teamLocations;
    public Transform location;

    public void Start() 
    {
        lobbySystem = FindObjectOfType<LobbySystem>();
        roomManager = FindObjectOfType<NetworkRoomManagerExt>();    
        // if (NetworkRoomManagerExt.singleton.GameplayScene == "Farmaze" && NetworkServer.activeHost )
        // {switchRoleButton.SetActive(true);}
    }

    public void BackToLobby()
    {
        lobbySystem.lobbyPanel.gameObject.SetActive(true);
        lobbySystem.OpenLobbyMenu();
    }

    public void ChangeReadyState()
    {
        roomPlayer.CmdChangeReadyState(!roomPlayer.readyToBegin);
    }

    public void ShowStartButton ()
    {
        Debug.Log("changing button to:  ");
        startbutton.SetActive(NetworkRoomManagerExt.singleton.allPlayersReady);
    }
    public void StartGame ()
    {
        if (!roomManager.allPlayersReady) {return;}
        roomManager.ServerChangeScene(roomManager.GameplayScene);
    }

    public void SetPlayerModel()
    {
        roomPlayer.SetModelName(modelName.options[modelName.value].text);
    }

    public void JoinTeam(int team) 
    {
        roomPlayer.playerTeam = team;
    }

    // public void ChangePlayerRoles()
    // {
    //     foreach (NetworkRoomPlayerExt player  in NetworkRoomManagerExt.singleton.roomSlots )
    //     {
    //         if (player.index == 0) {player.index = 1;}
    //         if (player.index == 1) {player.index = 0;}
    //     }
    // }


}
