using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class RoomPlayerUI : NetworkBehaviour 
{
    [SyncVar (hook = nameof(HandleNameChange))] public string pName;
    [SyncVar (hook = nameof(HandleStateChange))] public bool pState;
    public Text playerName;
    public Text playerState;
    public Image playerStateImage;
    public Sprite[] stateImages;

 
    public void HandleNameChange(string oldValue, string newValue)
    {
        playerState.text = pName;
    }


    public void HandleStateChange(bool oldValue, bool newValue)
    {
        //playerState.text = pState.ToString();
        if (pState) {playerStateImage.sprite = stateImages[1];}
        else {playerStateImage.sprite = stateImages[0];}
    }



}
