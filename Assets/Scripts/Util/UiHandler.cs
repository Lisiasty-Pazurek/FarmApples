using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiHandler : MonoBehaviour
{   
    public delegate void ChangeReadyState();
    public static event ChangeReadyState OnStateChange;
    public Text playerStateText;



    public void ChangeUIState()
    {
        OnStateChange?.Invoke();
    }

    public void OnEnable()
    {
        UiHandler.OnStateChange += ChangeStateText;
    }

    public void OnDisable()
    {
        UiHandler.OnStateChange -= ChangeStateText;
    }


    void ChangeStateText()
    {
        if (FindObjectOfType<UIData>().playerState)
        {playerStateText.text = "Ready";}
        else
        playerStateText.text = "Not ready";
    }

}
