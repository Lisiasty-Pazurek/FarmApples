using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MazeNavigator : NetworkBehaviour
{
    public Image buttonImage;
    public Image navigationImage;
    public enum navigationState {forward,left,right,backward,stop,smile,alert,yes,no};
    public List<Image> navigationImages;
    public Dictionary<navigationState,Image> navigationDictionary;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
 //       if (NetworkClient)

    }

    [ClientRpc]
    public void RPCStateIcon (int index)
    {
        index = GetComponent<PlayerController>().playerIndex - 1;
        
        ChangeNavigationIcon(index);
    }

    [ClientRpc]
    public void ChangeNavigationIcon(int index) 
    {
        if (index == GetComponent<PlayerController>().playerIndex)
        navigationImage.sprite = buttonImage.sprite; 
        navigationImage.enabled = true;
    }


}
