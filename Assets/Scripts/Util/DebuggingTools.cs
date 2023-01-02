using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Examples.NetworkRoom;

public class DebuggingTools : MonoBehaviour
{
    [SerializeField]
    private NetworkRoomManager networkManager;

    public void GetPlayersInfo()
    {
        
        // some WIP trash
        // if (NetworkManager.networkSceneName == "OnlineScene")
        // {
        //     foreach (NetworkRoomPlayer roomPlayer in roomSlots)
        //     {
        //         if (roomPlayer == null)
        //         continue;
        //     }
        // }
        // NetworkServer.connections.Count();
    }
}