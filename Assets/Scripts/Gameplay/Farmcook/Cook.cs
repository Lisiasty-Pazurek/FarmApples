using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using UnityEngine.UI;

public class Cook : NetworkBehaviour
{
    [SyncVar (hook =nameof(OnStaminaChange))]public float playerStamina;
    [SyncVar (hook =nameof(CarriedObjectChange))] public GameObject carriedObject;
    [SyncVar] public int teamID;

    public Transform rootTransform;
    public Slider staminaSlider;


    public void OnStaminaChange(float oldValue, float newValue)
    {
        staminaSlider.value = playerStamina;        
    }

    public void CarriedObjectChange(GameObject oldValue, GameObject newValue)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

 
    void Update()
    {
        if (isServer)
        {
            if (carriedObject != null)
            {
                playerStamina -= Time.deltaTime;
            }
            if (carriedObject == null && playerStamina <100)
            {
                playerStamina += Time.deltaTime/5;
            }
        }

        if (isClient)
        {
            if (Input.GetKey(KeyCode.F))
            {
                if (carriedObject != null)
                {
                    carriedObject.GetComponent<Ingredient>().DropItem(this.gameObject);                
                }

            }
        }
    }
}
