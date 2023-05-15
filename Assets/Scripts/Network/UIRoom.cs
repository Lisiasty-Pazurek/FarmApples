using UnityEngine;
using UnityEngine.UI;
using MirrorBasics;
using Mirror;
using System.Collections.Generic;

public class UIRoom : MonoBehaviour
{
    public static UIRoom instance {get; private set;}
    public LobbySystem lobbySystem;
    public NetworkRoomPlayerExt roomPlayer;
    public RoomPlayerUI roomPlayerUI;
    public NetworkRoomManagerExt roomManager;
    public Text readybutton;
    public GameObject startbutton;
    public Dropdown modelName;
    public InputField playerNameInput;
    
    public List<Transform> teamLocations;
    public Transform location;

    private void Awake() 
    {
        instance = this;
    }
    public void Start() 
    {
        lobbySystem = FindObjectOfType<LobbySystem>();
        roomManager = FindObjectOfType<NetworkRoomManagerExt>();    
        roomPlayer = NetworkRoomPlayerExt.localPlayer;
    }

    public void BackToLobby()
    {
        NetworkClient.Disconnect();
        NetworkServer.Shutdown();
        lobbySystem.lobbyPanel.gameObject.SetActive(true);
        lobbySystem.OpenLobbyMenu();
    }

    public void ChangeReadyState()
    {
        roomPlayer.CmdChangeReadyState(!roomPlayer.readyToBegin);
    }

    public void ShowStartButton ()
    {
        Debug.Log("changing start button to:  " + NetworkRoomManagerExt.singleton.allPlayersReady );
        startbutton.SetActive(!NetworkRoomManagerExt.singleton.allPlayersReady); // <--- derp
    }
    public void StartGame ()
    {
        if (!roomManager.allPlayersReady) {return;}
        roomManager.ServerChangeScene(roomManager.GameplayScene);
    }

    public void SetPlayerModel()
    {
        roomPlayer.CmdSetModelName(modelName.options[modelName.value].text);
    }

    public void SetPlayerName()
    {
        roomPlayer.CmdSetPlayerName(playerNameInput.text);
    }

    public void JoinTeam(int team) 
    {
        roomPlayer.CmdSetPlayerTeam(team);
    }

    public void SetPlayerModel2(string animal )
    {
        roomPlayer.CmdSetModelName(animal);
    }

    // Script for Farmaze gamemode to let host and clinet swap their roles
    // public void ChangePlayerRoles()
    // {
    //     foreach (NetworkRoomPlayerExt player  in NetworkRoomManagerExt.singleton.roomSlots )
    //     {
    //         if (player.index == 0) {player.index = 1;}
    //         if (player.index == 1) {player.index = 0;}
    //     }
    // }


    }