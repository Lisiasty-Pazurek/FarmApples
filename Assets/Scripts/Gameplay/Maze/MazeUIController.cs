using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MazeUIController : NetworkBehaviour
{
    public Image buttonImage;
    public Image navigationImage;
    public enum navigationState {forward,left,right,backward,stop,smile,alert,yes,no};

    public Dictionary<navigationState,Image> navigationDictionary;

    [ClientRpc]
    public void RPCStateIcon (int index)
    {
        index = GetComponent<PlayerController>().playerIndex - 1;
        
        ChangeNavigationIcon(index);
    }

    [TargetRpc]
    public void ChangeNavigationIcon(int index) 
    {
        if (index == GetComponent<PlayerController>().playerIndex )
        navigationImage.sprite = buttonImage.sprite; 
    }


}
