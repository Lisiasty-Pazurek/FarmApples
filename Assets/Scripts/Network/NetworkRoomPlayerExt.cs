using UnityEngine;
using Mirror;
using UnityEngine.UI;

namespace MirrorBasics
{
//    [AddComponentMenu("")]
    public class NetworkRoomPlayerExt : NetworkRoomPlayer
    {
        public static NetworkRoomPlayerExt localPlayer;
        public UIRoom uiRoom;

        [SyncVar] public string playerName;
        public string playerModel;
        public string playerTeam;
        public GameObject roomPlayerUIprefab;
        
        public override void OnStartClient()
        {
            //Debug.Log($"OnStartClient {gameObject}");
            uiRoom = FindObjectOfType<UIRoom>();
      
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            
            playerName = PlayerPrefs.GetString("PlayerName");
            
        }

        public override void OnClientEnterRoom()
        {
            //Debug.Log($"OnClientEnterRoom {SceneManager.GetActiveScene().path}");
            uiRoom = FindObjectOfType<UIRoom>();
            if (isLocalPlayer)
            {
                uiRoom.roomPlayer = this;
            }  
            GameObject roomPlayerUI = Instantiate(roomPlayerUIprefab,uiRoom.location);

            roomPlayerUI.GetComponent<RoomPlayerUI>().playerName.text= playerName;     
        }

        public override void OnClientExitRoom()
        {
            //Debug.Log($"OnClientExitRoom {SceneManager.GetActiveScene().path}");
        }

        public override void IndexChanged(int oldIndex, int newIndex)
        {
            //Debug.Log($"IndexChanged {newIndex}");
        }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            //Debug.Log($"ReadyStateChanged {newReadyState}"); Important!
            if (isLocalPlayer)
            {
                if (readyToBegin) { uiRoom.readybutton.text = "Gotowy"; }
                else { uiRoom.readybutton.text = "Nie gotowy"; }
            }
        
            //it's stupid but works - need coroutine fix
            if (isServer)
            {
                if (uiRoom != null) 
                {
                    uiRoom.ShowStartButton(!uiRoom.roomManager.allPlayersReady);
                    Debug.Log("Changed button visibility to: " + !uiRoom.roomManager.allPlayersReady);
                }
            }
        }


        public override void OnGUI()
        {
            base.OnGUI();
        }
    }
}
