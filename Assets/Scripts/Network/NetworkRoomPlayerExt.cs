using UnityEngine;
using Mirror;
namespace MirrorBasics
{
//    [AddComponentMenu("")]
    public class NetworkRoomPlayerExt : NetworkRoomPlayer
    {
        public static NetworkRoomPlayerExt localPlayer;
        public UIRoom uiRoom;

        [SyncVar] public string playerName;
        public GameObject localRoomPlayerUi;
        public string playerModel;
        public string playerTeam;
        public GameObject roomPlayerUIprefab;
        
        public override void OnStartClient()
        {
            //Debug.Log($"OnStartClient {gameObject}");
            uiRoom = FindObjectOfType<UIRoom>();
            playerName = NetworkRoomManagerExt.singleton.lobbySystem.playerNameInputField.text;     
            playerModel = uiRoom.modelName.options[uiRoom.modelName.value].text;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            uiRoom = FindObjectOfType<UIRoom>();
            SpawnRoomUIPrefab();            
        }

        public override void OnClientEnterRoom()
        {
            //Debug.Log($"OnClientEnterRoom {SceneManager.GetActiveScene().path}");
            uiRoom = FindObjectOfType<UIRoom>();

            if (isClient)
            {
                if (isLocalPlayer)
                {
                    uiRoom.roomPlayer = this;
                }    
            }

        }

        public void SetModelName(string modelName)
        {
            playerModel = modelName;
        }        
        
        [Server]
        public void SpawnRoomUIPrefab ()
        {
            if (isServer) 
            {
                GameObject roomPlayerUI = Instantiate(roomPlayerUIprefab,uiRoom.location);
                roomPlayerUI.GetComponent<RoomPlayerUI>().thisPlayer = this.gameObject;
                NetworkServer.Spawn(roomPlayerUI);
                localRoomPlayerUi = roomPlayerUI;
            }
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
            // changing button text locally
            if (isLocalPlayer)
            {
                if (readyToBegin) { uiRoom.readybutton.text = "Gotowy"; }
                else { uiRoom.readybutton.text = "Nie gotowy"; }
            }

            if (isClient)
            {
                if (localRoomPlayerUi != null)
                localRoomPlayerUi.GetComponent<RoomPlayerUI>().pState = newReadyState;
                //localRoomPlayerUi.GetComponent<RoomPlayerUI>();
            }
        
            // changing visibility of start button for host
            if (isServer)
            {
                if (uiRoom != null) 
                {
                    uiRoom.ShowStartButton(NetworkRoomManagerExt.singleton.allPlayersReady);           
                }
            }
        }


        public override void OnGUI()
        {
            base.OnGUI();
        }
    }
}
