using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public Text playerName;

    public void SetPlayerName()
    {
        string pName = playerName.text.ToString(); 
        PlayerPrefs.SetString("PlayerName", pName);
    }


}
