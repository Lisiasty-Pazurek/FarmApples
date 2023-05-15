using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MirrorBasics;

public class PlayerGameUI : MonoBehaviour
{
    public Camera pCamera;
    private Canvas gameUICanvas;
    [SerializeField] private Text playerName;
    [SerializeField] private Image namePlate;
    
    
    public void Start() 
    {
        pCamera = Camera.main;
        gameUICanvas = GetComponent<Canvas>();
        gameUICanvas.worldCamera = pCamera;
        playerName.text = GetComponentInParent<PlayerController>().playerName;
        SetTeam();
    }

    public void SetTeam()
    {
        if (GetComponentInParent<PlayerScore>().teamID == 1)
        {
            namePlate.color = new Color( .5f, .8f, 1, 1);
            print("color 1");
        }
        if (GetComponentInParent<PlayerScore>().teamID == 2)
        {
            namePlate.color = new Color(1, .5f, .5f, 1);
            print("color 2");
        }        
    }

    private void Update() 
    {
        transform.LookAt(transform.position + pCamera.transform.rotation * Vector3.forward,
        pCamera.transform.rotation * Vector3.up);
    }

}
