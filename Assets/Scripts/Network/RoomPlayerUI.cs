using UnityEngine;
using UnityEngine.UI;
using Mirror;
using MirrorBasics;

public class RoomPlayerUI : NetworkBehaviour 
{
    [SyncVar (hook = nameof(HandleNameChange))] public string pName;
    [SyncVar (hook = nameof(HandleStateChange))] public bool pState;
    [SyncVar] public int index;
    public Text playerName;
    public Text playerState;

    public GameObject roomPlayer;

    public Image playerStateImage;
    public Sprite[] stateImages;

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log(" before loopstart of  " + NetworkRoomManagerExt.singleton.roomSlots.Count);

        foreach (NetworkRoomPlayer player in NetworkRoomManagerExt.singleton.roomSlots)
        {
            Debug.Log("start of loop for asigning room players in roomplayer ui ");
            if (player.index == index) 
            { 
                roomPlayer = player.gameObject;
                roomPlayer.GetComponent<NetworkRoomPlayerExt>().localRoomPlayerUi = this.gameObject;
                Debug.Log("assigning room players in roomplayer ui for player of index: " + index);
            }
        }

        if (FindObjectOfType<UIRoom>() != null)
        {
            this.transform.SetParent(FindObjectOfType<UIRoom>().location);
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        roomPlayer.GetComponent<NetworkRoomPlayerExt>().localRoomPlayerUi = this.gameObject;
        Debug.Log("assigning localplaeyr ui for assigned room player");
    }

    public override void OnStartServer ()
    {
 //       pName = roomPlayer.GetComponent<NetworkRoomPlayerExt>().playerName;
    }

    public void HandleNameChange(string oldValue, string newValue)
    {
        playerName.text = pName;
    }


    public void HandleStateChange(bool oldValue, bool newValue)
    {
        //playerState.text = pState.ToString();
        if (pState) {playerStateImage.sprite = stateImages[1];}
        else {playerStateImage.sprite = stateImages[0];}
    }


    public void RpcMovePlayerPrefabToTeam(int team)
    {
        this.transform.SetParent(FindObjectOfType<UIRoom>().teamLocations[team].transform);
    }

}
