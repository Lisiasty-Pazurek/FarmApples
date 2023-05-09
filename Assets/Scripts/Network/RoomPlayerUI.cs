using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomPlayerUI : MonoBehaviour 
{

    public int index;
    public bool pState;
    public Text playerName;
    public string playerModel;
    public int playerTeam;

    public GameObject roomPlayer;

    public Image playerStateImage;
    public Image playerModelImage;
    public Sprite modelImage;

    public List<Sprite> modelImages;
    public Sprite[] stateImages;

    private void Start() 
    {
        this.transform.SetParent(FindObjectOfType<UIRoom>().teamLocations[playerTeam].transform);
    }

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
        foreach (Sprite sprite in modelImages)
        {
            if (sprite.name == newModel)
            modelImage = sprite;
        }

        if (newModel =="")
        {   modelImage = modelImages[0];  }

        playerModelImage.sprite = modelImage;  


    }
    public void OnPlayerTeamChanged(int newTeam)
    {
        playerTeam = newTeam;
        this.transform.SetParent(FindObjectOfType<UIRoom>().teamLocations[newTeam].transform);
    }
        



}
