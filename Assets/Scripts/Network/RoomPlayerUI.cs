using UnityEngine;
using UnityEngine.UI;
using Mirror;
using MirrorBasics;

public class RoomPlayerUI : MonoBehaviour 
{

    public int index;
    public bool pState;
    public Text playerName;
    public string playerModel;
    public Text playerState;
    public int playerTeam;

    public GameObject roomPlayer;

    public Image playerStateImage;
    public Image playerModelImage;
    public Sprite[] stateImages;


    public void OnPlayerIndexChanged()
    {

    }
    public void OnPlayerStateChanged( bool newValue)
    {
        pState = newValue;
        if (pState) {playerStateImage.sprite = stateImages[1];}
        else {playerStateImage.sprite = stateImages[0];}
    }
    public void OnPlayerNameChanged(string newName)
    {
        playerName.text = newName;
    }
    public void OnPlayerModelChanged(string newModel)
    {
        playerModel = newModel;
    }
    public void OnPlayerTeamChanged(int newTeam)
    {
        playerTeam = newTeam;
        this.transform.SetParent(FindObjectOfType<UIRoom>().teamLocations[newTeam].transform);
    }
        



}
