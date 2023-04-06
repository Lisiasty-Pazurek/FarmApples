using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/
namespace MirrorBasics {
[AddComponentMenu("")]
    public class NetworkRoomManagerExt : NetworkRoomManager
    {

        public static new NetworkRoomManagerExt singleton { get; private set; }

        public LobbySystem lobbySystem;
        /// <summary>
        /// Runs on both Server and Client
        /// Networking is NOT initialized when this fires
        /// </summary>
        public override void Awake()
        {
            base.Awake();
            singleton = this;
        }

        public override void OnStartHost()
        {
            onlineScene = RoomScene;
            base.OnStartHost();
        }

        /// <summary>
        /// This is called on the server when a networked scene finishes loading.
        /// </summary>
        /// <param name="sceneName">Name of the new scene.</param>
        public override void OnRoomServerSceneChanged(string sceneName)
        {
            if (sceneName == RoomScene)
            { 
                foreach (NetworkRoomPlayerExt player in roomSlots)
                {
                    player.uiRoom = FindObjectOfType<UIRoom>();
                    player.roomPlayerUI.transform.SetParent(player.uiRoom.location.transform);
                }
            }
        }

        public override void ServerChangeScene(string newSceneName)
        {
            base.ServerChangeScene(newSceneName);
            if (newSceneName == GameplayScene)
            { 
                foreach (NetworkRoomPlayerExt player in roomSlots)
                {
                    player.roomPlayerUI.transform.SetParent(NetworkRoomManagerExt.singleton.transform);
                }
            }
        }

        public override void OnRoomClientEnter() 
        {
            
        }

        /// <summary>
        /// Called just after GamePlayer object is instantiated and just before it replaces RoomPlayer object.
        /// This is the ideal point to pass any data like player name, credentials, tokens, colors, etc.
        /// into the GamePlayer object as it is about to enter the Online scene.
        /// </summary>
        /// <param name="roomPlayer"></param>
        /// <param name="gamePlayer"></param>
        /// <returns>true unless some code in here decides it needs to abort the replacement</returns>
        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            /// additional class for changing player objects depending on game mode seems necessary for clarity
            if (SceneManager.GetActiveScene().name == "Apples01") 
            {
                PlayerScore playerScore = gamePlayer.GetComponent<PlayerScore>();
                PlayerController playerGameController = gamePlayer.GetComponent<PlayerController>();
                playerScore.index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
                if (IsOdd(roomPlayer.GetComponent<NetworkRoomPlayer>().index))
                {
                    playerGameController.modelName = "Sheep";
                    playerScore.teamID = 1;
                }
                else 
                {
                    playerGameController.modelName = "Donkey";
                    playerScore.teamID = 2;
                }
                return true;
            }
            else if (SceneManager.GetActiveScene().name == "DialogueScene")
            {
                PlayerScore playerScore = gamePlayer.GetComponent<PlayerScore>();
                PlayerController playerGameController = gamePlayer.GetComponent<PlayerController>();
                playerScore.index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
                playerGameController.playerIndex = playerScore.index;
                playerGameController.modelName = "Sheep";
                playerGameController.GetComponent<PlayerInteractions>().canInteract = true;

                return true;
            }

            else if (SceneManager.GetActiveScene().name == "Farmaze")
            {
                PlayerScore playerScore = gamePlayer.GetComponent<PlayerScore>();
                PlayerController playerGameController = gamePlayer.GetComponent<PlayerController>();
                playerScore.index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
                if (!IsOdd(roomPlayer.GetComponent<NetworkRoomPlayer>().index))
                {
                    playerGameController.modelName = roomPlayer.GetComponent<NetworkRoomPlayerExt>().playerModel;
                    playerGameController.jumpPower = 1;
                    
                }
                else 
                {
                    playerGameController.modelName = "Empty";
                    playerScore.isNavigator = true;
                    
                }
                return true;
            }

            else if (SceneManager.GetActiveScene().name == "Farmarathon")
            {
                PlayerScore playerScore = gamePlayer.GetComponent<PlayerScore>();
                PlayerController playerGameController = gamePlayer.GetComponent<PlayerController>();
                playerScore.index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
                playerGameController.playerName = roomPlayer.GetComponent<NetworkRoomPlayerExt>().playerName;
                playerGameController.modelName = roomPlayer.GetComponent<NetworkRoomPlayerExt>().playerModel;
                return true;
            }
            else return false;
        }

        public override void OnRoomStopClient()
        {
            base.OnRoomStopClient();
        }

        public override void OnRoomStopServer()
        {
            base.OnRoomStopServer();
        }

        public static bool IsOdd(int value)
        {
            return value % 2 != 0;
        }

        /*
            This code below is to demonstrate how to do a Start button that only appears for the Host player
            showStartButton is a local bool that's needed because OnRoomServerPlayersReady is only fired when
            all players are ready, but if a player cancels their ready state there's no callback to set it back to false
            Therefore, allPlayersReady is used in combination with showStartButton to show/hide the Start button correctly.
            Setting showStartButton false when the button is pressed hides it in the game scene since NetworkRoomManager
            is set as DontDestroyOnLoad = true.
        */

        public bool showStartButton;

        public override void OnRoomServerPlayersReady()
        {
            // calling the base method calls ServerChangeScene as soon as all players are in Ready state.
#if UNITY_SERVER
            NetworkClient.localPlayer.uiRoom.ShowStartButton();
            base.OnRoomServerPlayersReady();
#else
            //showStartButton = true;
            Debug.Log("for server - all players ready");
#endif
        }

        public override void OnGUI()
        {
            base.OnGUI();

            if (showRoomGUI && allPlayersReady && showStartButton && GUI.Button(new Rect(Screen.width - 160, 300, 120, 20), "START"))
            {
                // set to false to hide it in the game scene
                showStartButton = false;
                ServerChangeScene(GameplayScene);
            }
        }


    public override void OnClientConnect()
    {
        lobbySystem.lobbyPanel.gameObject.SetActive(false);
        base.OnClientConnect();
    }

    public override void OnClientDisconnect()
    {
        lobbySystem.lobbyPanel.gameObject.SetActive(true);
        lobbySystem.RefreshServerList();
        base.OnClientDisconnect();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
        if(lobbySystem.LRMTransport.ClientConnected())
        lobbySystem.LRMTransport.DisconnectFromRelay();
        SceneManager.LoadScene("LobbySample");
        Destroy(this.gameObject);
    }


    }
}
