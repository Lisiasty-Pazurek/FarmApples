using UnityEngine;
using UnityEngine.UI;
using Mirror;
using MirrorBasics;

public class RoomPlayerUI : NetworkBehaviour 
{
    [SyncVar (hook = nameof(HandleNameChange))] public string pName;
    [SyncVar (hook = nameof(HandleStateChange))] public bool pState;
    public Text playerName;
    public Text playerState;

    public NetworkRoomPlayerExt thisPlayer;

    public Image playerStateImage;
    public Sprite[] stateImages;

    public override void OnStartClient()
    {
        base.OnStartClient();
        this.transform.SetParent(FindObjectOfType<UIRoom>().location);
    }

    public override void OnStartServer ()
    {
        pName = thisPlayer.playerName;
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



}
