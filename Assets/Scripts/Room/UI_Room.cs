using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using MirrorBasics;

public class UI_Room : MonoBehaviour
{
    LobbySystem lobbySystem;
    private void Start() 
    {
        lobbySystem = FindObjectOfType<LobbySystem>();
    }
    

    public void BackToLobby()
    {
        lobbySystem.OpenLobbyMenu();
    }


}
