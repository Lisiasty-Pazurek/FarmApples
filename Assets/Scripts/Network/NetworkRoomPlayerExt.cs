using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

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

        [SyncVar (hook = nameof(PlayerIndexChanged))] public int playerIndex;
        [SyncVar (hook = nameof(PlayerNameChanged))] public string playerName;
        [SyncVar (hook = nameof(PlayerModelChanged))] public string playerModel;
        [SyncVar (hook = nameof(PlayerTeamChanged))] public int playerTeam;

        //public GameObject localRoomPlayerUi;        
        public GameObject roomPlayerUIprefab;
        public GameObject roomPlayerUIObject;
        public RoomPlayerUI roomPlayerUI;
        
        public override void OnStartClient()
        {
            //Debug.Log($"OnStartClient {gameObject}");
            uiRoom = FindObjectOfType<UIRoom>();    
            //playerModel = uiRoom.modelName.options[uiRoom.modelName.value].text;

            InstantiateRoomUIPrefab();
            base.OnStartClient();
        }



        public void InstantiateRoomUIPrefab()
        {
            // Instantiate the player UI as child of the Players Panel
            if (uiRoom != null)
            {
                roomPlayerUIObject = Instantiate(roomPlayerUIprefab, uiRoom.location);
                roomPlayerUI = roomPlayerUIObject.GetComponent<RoomPlayerUI>();            

                // wire up all events to handlers in PlayerUI
                OnPlayerNameChanged = roomPlayerUI.OnPlayerNameChanged;
                OnPlayerModelChanged = roomPlayerUI.OnPlayerModelChanged;
                OnPlayerTeamChanged = roomPlayerUI.OnPlayerTeamChanged;
                OnPlayerStateChanged = roomPlayerUI.OnPlayerStateChanged;  

                // Load Initial prefab data
                OnPlayerNameChanged?.Invoke(playerName);
                OnPlayerModelChanged?.Invoke(playerModel);
                OnPlayerTeamChanged?.Invoke(playerTeam);
                OnPlayerStateChanged?.Invoke(readyToBegin);
            }
        }

        void PlayerIndexChanged(int oldName, int newName)
        {   
            // if (roomPlayerUI != null)
            // OnPlayerNameChanged?.Invoke(newName);
        }

        void PlayerNameChanged(string oldName, string newName)
        {   
            if (roomPlayerUI != null)
            OnPlayerNameChanged?.Invoke(newName);
        }
        void PlayerModelChanged(string oldModel, string newModel)
        {
            if (roomPlayerUI != null)
            OnPlayerModelChanged?.Invoke(newModel);
        }

        void PlayerTeamChanged(int oldTeam, int newTeam)
        {
            if (roomPlayerUI != null)
            OnPlayerTeamChanged?.Invoke(newTeam);
        }

        public override void OnStartServer()
        {
            base.OnStartServer();             
            playerName = "Gracz " + Random.Range(0, 999).ToString();
            playerIndex = index + 1;
            uiRoom = FindObjectOfType<UIRoom>();

            if (SceneManager.GetActiveScene().name == "RoomSceneMarathon" )
            {
                playerModel = FindObjectOfType<CharacterPicker>().modelSprites[Random.Range(0, FindObjectOfType<CharacterPicker>().modelSprites.Count)].name;
                Debug.Log("player model of : " + playerModel);
            }       
            if (SceneManager.GetActiveScene().name == "RoomSceneCook" )
            {
                playerModel = NetworkManager.singleton.playerPrefab.GetComponent<PlayerController>().characterModel[Random.Range(0, NetworkManager.singleton.playerPrefab.GetComponent<PlayerController>().characterModel.Count)].name;
                Debug.Log("player model of : " + playerModel);
            }       
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
                    if (uiRoom != null) 
                    {
                        uiRoom.roomPlayer = this;

                        if (roomPlayerUIObject == null)
                        {
                            InstantiateRoomUIPrefab();
                        } 
                    }   
                }
            }

        }

        [Command]
        public void CmdSetModelName(string modelName)
        {
            playerModel = modelName;
        }       

        [Command]
        public void CmdSetPlayerName (string pName)
        {
            playerName = pName;
        }

        [Command]
        public void CmdSetPlayerTeam (int teamNumber)
        {
            playerTeam = teamNumber;
        }

        
        public override void OnClientExitRoom()
        {
            //Debug.Log($"OnClientExitRoom {SceneManager.GetActiveScene().path}");
            Destroy(roomPlayerUIObject);
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
                if (uiRoom != null)
                {
                    if (readyToBegin) { uiRoom.readybutton.text = "Gotowy"; }
                    else { uiRoom.readybutton.text = "Nie gotowy"; }
                }
            }

            if (isClient)
            {
                if (uiRoom != null)
                {
                    OnPlayerStateChanged?.Invoke(newReadyState);
                }
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
