using UnityEngine;
using Mirror;

namespace MirrorBasics
{
    public class NetworkRoomPlayerExt : NetworkRoomPlayer
    {
        public static NetworkRoomPlayerExt localPlayer;
        public UIRoom uiRoom;

        [SyncVar] public string playerName;
        [SyncVar] public string playerModel;
        [SyncVar] public int playerTeam;

        public GameObject localRoomPlayerUi;        
        public GameObject roomPlayerUIprefab;
        
        public override void OnStartClient()
        {
            //Debug.Log($"OnStartClient {gameObject}");
            uiRoom = FindObjectOfType<UIRoom>();
            playerName = NetworkRoomManagerExt.singleton.lobbySystem.playerNameInputField.text;     
            playerModel = uiRoom.modelName.options[uiRoom.modelName.value].text;
          
        }

        public void AssignRoomPlayerObject()
        {

        }


        public override void OnStartServer()
        {
            base.OnStartServer();
            uiRoom = FindObjectOfType<UIRoom>();
            SpawnRoomUIPrefab(this.index, this.gameObject);
            Debug.Log("Spawning ui prefab for: " + index + " " + this.gameObject.name);   
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
        public void SpawnRoomUIPrefab (int playerIndex, GameObject roomPlayerObject)
        {
            Debug.Log("Spawning room prefab for: "+ index + " " + this.gameObject.name + " re " + playerIndex + roomPlayerObject.name);
            GameObject roomPlayerUI = Instantiate(roomPlayerUIprefab,uiRoom.location);
            NetworkServer.Spawn(roomPlayerUI);
            roomPlayerUI.GetComponent<RoomPlayerUI>().index = playerIndex;
            roomPlayerUI.GetComponent<RoomPlayerUI>().roomPlayer = roomPlayerObject;
            RPCRoomPlayerUIPrefab(roomPlayerUI, playerIndex, roomPlayerObject);
            localRoomPlayerUi = roomPlayerUI;
        }

        [ClientRpc]
        public void RPCRoomPlayerUIPrefab (GameObject roomPlayeruiObject,int playerIndex, GameObject roomPlayerObject)
        {
            Debug.Log("Setting up room prefab for: "+ roomPlayeruiObject + "object " + playerIndex + " index" + roomPlayerObject.name);
            roomPlayeruiObject.GetComponent<RoomPlayerUI>().index = playerIndex;
            roomPlayeruiObject.GetComponent<RoomPlayerUI>().roomPlayer = roomPlayerObject;

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
            }
        
            //changing visibility of start button for host
            if (isServer)
            {
                if (uiRoom != null) 
                {
//                    uiRoom.startbutton.SetActive(newReadyState);     
                    uiRoom.ShowStartButton();      
                }
            }
        }


        public override void OnGUI()
        {
            base.OnGUI();
        }
    }
}
