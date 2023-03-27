using UnityEngine;
using UnityEngine.UI;
using MirrorBasics;


public class UIRoom : MonoBehaviour
{
    public static UIRoom singleton {get; private set;}
    public LobbySystem lobbySystem;
    public NetworkRoomPlayerExt roomPlayer;
    public NetworkRoomManagerExt roomManager;
    public Text readybutton;
    public GameObject startbutton;
    public Dropdown modelName;
    

    [SerializeField] public Transform location;

    public void Start() 
    {
        lobbySystem = FindObjectOfType<LobbySystem>();
        roomManager = FindObjectOfType<NetworkRoomManagerExt>();    
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

    public void ShowStartButton (bool state)
    {
        // that's cheating but it changes state at same moment as it shows button
        Debug.Log("changing button to:  " + state);
        startbutton.SetActive(!state);
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


}
