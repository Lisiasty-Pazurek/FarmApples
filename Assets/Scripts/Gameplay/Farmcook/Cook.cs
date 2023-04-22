using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Cook : NetworkBehaviour
{
    [SyncVar (hook =nameof(OnStaminaChange))]public float playerStamina;
    [SyncVar (hook =nameof(CarriedObjectChange))] public GameObject carriedObject;
    public Transform rootTransform;

    

    public void OnStaminaChange(float oldValue, float newValue)
    {
        
    }

    public void CarriedObjectChange(GameObject oldValue, GameObject newValue)
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
