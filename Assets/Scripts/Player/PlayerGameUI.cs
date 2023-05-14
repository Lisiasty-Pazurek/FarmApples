using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerGameUI : MonoBehaviour
{
    public Camera pCamera;
    private Canvas gameUICanvas;
    [SerializeField] private Text playerName;
    
    
    public void Start() 
    {
        pCamera = Camera.main;
        gameUICanvas = GetComponent<Canvas>();
        gameUICanvas.worldCamera = pCamera;
        playerName.text = GetComponentInParent<PlayerController>().playerName;
    }

    private void Update() 
    {
        transform.LookAt(transform.position + pCamera.transform.rotation * Vector3.forward,
        pCamera.transform.rotation * Vector3.up);
    }

}
