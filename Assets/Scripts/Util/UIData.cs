using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIData : MonoBehaviour
{
    public bool playerState;

    public void OnEnable()
    {
        UiHandler.OnStateChange += ChangeState;
    }
    void OnDisable()
    {
        UiHandler.OnStateChange -= ChangeState;
    }

    void ChangeState() 
    {
        playerState = !playerState;
    }



    

}
