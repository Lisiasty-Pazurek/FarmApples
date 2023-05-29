using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using MirrorBasics;
public class LobbySystem : MonoBehaviour
{
    public static LobbySystem singleton { get; private set; }
    [SerializeField] public NetworkRoomManagerExt networkManager;
    public LightReflectiveMirror.LightReflectiveMirrorTransport LRMTransport;

    [Header("Lobby Settings")]
    public bool autoRefeshServerList;
    public int refreshServerListTimer = 10;

    [Header("Lobby Menu")]
    public Transform LobbyMenuParent;
    public Text lobbyMenuText;
    public InputField joinRoomNameInputField;
    public Transform lobbyPanel;
    public Transform templateListing;
    public InputField playerNameInputField;

    [Header("Create A Room Menu")]
    public Transform createRoomMenuParent;
    public InputField roomNameInputField;
    public Slider maxPlayersSlider;
    public Text maxPlayersText;
    public Dropdown mapListDropdown;
    // Start is called before the first frame update
    void Awake()
    {
        roomNameInputField.text = "Gra " + Random.Range(0, 999).ToString();
        playerNameInputField.text = "Gracz " + Random.Range(0, 999).ToString();
        Application.targetFrameRate = 60;
    }

    float listTimer = 0;
    private void Update()
    {
        if (autoRefeshServerList)
        {
            listTimer += 1 * Time.deltaTime;
            if(listTimer>= refreshServerListTimer)
            {
                listTimer = 0;
                if(!networkManager.isNetworkActive)RefreshServerList();
            }
        }

        maxPlayersText.text = "Limit graczy: " + maxPlayersSlider.value.ToString();
    }

    public void OpenLobbyMenu()
    {
        createRoomMenuParent.gameObject.SetActive(false);
        LobbyMenuParent.gameObject.SetActive(true);
    }

    public void OpenCreateRoomMenu()
    {
        createRoomMenuParent.gameObject.SetActive(true);
        LobbyMenuParent.gameObject.SetActive(false);
    }

    public void RefreshServerList()
    {
        LRMTransport.RequestServerList();
    }

    public void RoomSet()
    {
        if (mapListDropdown.options[mapListDropdown.value].text == "Farmaze")
        {
            maxPlayersSlider.maxValue = 2;
            maxPlayersSlider.value = 2;
            networkManager.playerPrefab = GetComponentInParent<GameModeSystem>().playerPrefabs[0];
        }

        if (mapListDropdown.options[mapListDropdown.value].text == "Farmapples")
        {
            maxPlayersSlider.maxValue = 20;
            maxPlayersSlider.value = 20;
            networkManager.playerPrefab = GetComponentInParent<GameModeSystem>().playerPrefabs[0];
        }

        if (mapListDropdown.options[mapListDropdown.value].text == "Farmarathon")
        {
            maxPlayersSlider.maxValue = 24;
            maxPlayersSlider.value = 24;
            networkManager.onlineScene = "RoomSceneMarathon";
            networkManager.RoomScene = "RoomSceneMarathon";
            networkManager.playerPrefab = GetComponentInParent<GameModeSystem>().playerPrefabs[1];
        }  

        if (mapListDropdown.options[mapListDropdown.value].text == "Farmhand")
        {
            maxPlayersSlider.maxValue = 20;
            maxPlayersSlider.value = 20;
            networkManager.onlineScene = "RoomSceneCook";
            networkManager.RoomScene = "RoomSceneCook";
            networkManager.playerPrefab = GetComponentInParent<GameModeSystem>().playerPrefabs[2];
        }

        if (mapListDropdown.options[mapListDropdown.value].text == "Farmcook")
        {
            maxPlayersSlider.maxValue = 20;
            maxPlayersSlider.value = 20;
            networkManager.onlineScene = "RoomSceneCook";
            networkManager.RoomScene = "RoomSceneCook";
            networkManager.playerPrefab = GetComponentInParent<GameModeSystem>().playerPrefabs[2];
        }


    }

    public void CreateRoom()
    {
        if(roomNameInputField.text.Length < 1) roomNameInputField.text = "Gra " + Random.Range(0, 999).ToString();

        LRMTransport.serverName = roomNameInputField.text;
        LRMTransport.maxServerPlayers = (int)maxPlayersSlider.value;
        LRMTransport.extraServerData = mapListDropdown.options[mapListDropdown.value].text;
        networkManager.StartHost();

        /// ### BEGIN ### custom changes for room -> gameplayscene changes
        networkManager.GameplayScene = mapListDropdown.options[mapListDropdown.value].text;
        Debug.Log("gameplay scene : " + mapListDropdown.options[mapListDropdown.value].text);
        networkManager.maxConnections = ((int)maxPlayersSlider.value-1);
        //networkManager.ServerChangeScene(networkManager.RoomScene);

        /// ### END ### custom changes for room -> gameplayscene changes
    }

    public void JoinRoom()
    {

        LRMTransport.RequestServerList();
        for (int i = 0; i < LRMTransport.relayServerList.Count; i++)
        {
            if (LRMTransport.relayServerList[i].serverName.Normalize() == joinRoomNameInputField.text.Normalize())
            {
                string serverID = LRMTransport.relayServerList[i].serverId;
                NetworkManager.singleton.networkAddress = serverID.ToString(); NetworkManager.singleton.StartClient();
                return;
            }
        }
    }

    public void OnServerListUpdated()
    {
        foreach (Transform t in templateListing.parent)
            if(t.gameObject.activeSelf)Destroy(t.gameObject);

        int totalPlayers = 0;
        for (int i = 0; i < LRMTransport.relayServerList.Count; i++)
        {
            print(LRMTransport.relayServerList[i].relayInfo.address + " ADDRESS");

            Transform roomEntry = Instantiate(templateListing, templateListing.parent);

            roomEntry.Find("RoomText").GetComponent<Text>().text = LRMTransport.relayServerList[i].serverName;
            roomEntry.Find("DataText").GetComponent<Text>().text = LRMTransport.relayServerList[i].serverData;
            roomEntry.Find("PlayerText").GetComponent<Text>().text = LRMTransport.relayServerList[i].currentPlayers.ToString() + "/"+ LRMTransport.relayServerList[i].maxPlayers.ToString();
            string serverID = LRMTransport.relayServerList[i].serverId;
            roomEntry.Find("JoinButton").GetComponent<Button>().onClick.AddListener(() => { NetworkManager.singleton.networkAddress = serverID.ToString(); NetworkManager.singleton.StartClient(); });
            roomEntry.gameObject.SetActive(true);
            totalPlayers += LRMTransport.relayServerList[i].currentPlayers;
        }

        lobbyMenuText.text = "Rozgrywki - " + totalPlayers.ToString() + " graczy online";

    }

    public void CheckRelayStatus()
    {

    }

    public void OnConnectedToRelay()
    {
        print("Connected to relay!");
        RefreshServerList();
    }

    public void OnDisconnectedFromRelay()
    {
        print("Disconnected from relay!");
    }
}
