using UnityEngine;
using Mirror;

namespace MirrorBasics
{
    public class NetworkRoomPlayerExt : NetworkRoomPlayer
    {
        public static NetworkRoomPlayerExt localPlayer;
        public UIRoom uiRoom;

        public event System.Action<int> OnPlayerIndexChanged;
        public event System.Action<bool> OnPlayerStateChanged;
        public event System.Action<string> OnPlayerNameChanged;
        public event System.Action<string> OnPlayerModelChanged;
        public event System.Action<int> OnPlayerTeamChanged;

        [SyncVar (hook = nameof(PlayerNameChanged))] public string playerName;
        [SyncVar (hook = nameof(PlayerModelChanged))] public string playerModel;
        [SyncVar (hook = nameof(PlayerTeamChanged))] public int playerTeam;

        public GameObject localRoomPlayerUi;        
        public GameObject roomPlayerUIprefab;
        public RoomPlayerUI roomPlayerUI;
        
        public override void OnStartClient()
        {
            //Debug.Log($"OnStartClient {gameObject}");
            uiRoom = FindObjectOfType<UIRoom>();
            playerName = NetworkRoomManagerExt.singleton.lobbySystem.playerNameInputField.text;     
            playerModel = uiRoom.modelName.options[uiRoom.modelName.value].text;
          
            // Instantiate the player UI as child of the Players Panel
            roomPlayerUIprefab = Instantiate(roomPlayerUIprefab, uiRoom.location);
            roomPlayerUI = roomPlayerUIprefab.GetComponent<RoomPlayerUI>();

            // wire up all events to handlers in PlayerUI
            OnPlayerNameChanged = roomPlayerUI.OnPlayerNameChanged;
            OnPlayerModelChanged = roomPlayerUI.OnPlayerModelChanged;
            OnPlayerTeamChanged = roomPlayerUI.OnPlayerTeamChanged;
            OnPlayerStateChanged = roomPlayerUI.OnPlayerStateChanged;

        }


        void PlayerNameChanged(string oldName, string newName)
        {
            OnPlayerNameChanged?.Invoke(newName);
        }
        void PlayerModelChanged(string oldModel, string newModel)
        {
            OnPlayerModelChanged?.Invoke(newModel);
        }

        void PlayerTeamChanged(int oldTeam, int newTeam)
        {
            OnPlayerTeamChanged?.Invoke(newTeam);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();
            uiRoom = FindObjectOfType<UIRoom>();
//            SpawnRoomUIPrefab(this.index, this.gameObject);
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
                OnPlayerStateChanged?.Invoke(newReadyState);
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
