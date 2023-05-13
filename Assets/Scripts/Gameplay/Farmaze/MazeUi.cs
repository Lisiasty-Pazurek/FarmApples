
using System.Collections.Generic;
using UnityEngine;

public class MazeUi : MonoBehaviour
{
    public MazeNavigator mazeNavigation;
    int navigationState;

    private void Start() 
    {
        mazeNavigation = GetComponent<MazeNavigator>();
    }

    public void SendNavigation()
    {
        mazeNavigation.CmdStateIcon(navigationState);
    }

}
