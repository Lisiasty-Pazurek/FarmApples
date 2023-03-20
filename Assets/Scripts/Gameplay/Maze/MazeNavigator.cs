using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class MazeNavigator : NetworkBehaviour
{
    public static MazeNavigator mazeNavigation;
    public Image buttonImage;
    public Image navigationImage;
    private int imageId;
    public enum navigationState {forward = 0,left = 1,right = 2,backward = 3,stop = 4,smile = 5,alert = 6,yes = 7,no = 8};
    public List<Image> navigationImages;


    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        mazeNavigation = this;
 //       if (NetworkClient)

    }



    [Command (requiresAuthority = false)]
    public void CmdStateIcon (int navigation)
    {
//        int index = GetComponent<PlayerController>().playerIndex - 1;
        imageId = navigation;
        
        RpcChangeNavigationIcon( imageId);
    }

    [ClientRpc]
    public void RpcChangeNavigationIcon( int imageId) 
    {
        Debug.Log("Rpc icon to all clients");
 //       if (index == GetComponent<PlayerController>().playerIndex)
        navigationImage.sprite = navigationImages[imageId].sprite; 
        navigationImage.enabled = true;
    }




}
