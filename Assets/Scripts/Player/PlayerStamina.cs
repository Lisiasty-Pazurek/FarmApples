using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerStamina : NetworkBehaviour
{
    [SyncVar (hook =nameof(OnStaminaChange))]public float playerStamina;
    public Carrier pCarry;
    public Transform rootTransform;
    public Slider staminaSlider;

    public void Start() 
    {
        pCarry = GetComponent<Carrier>();
    }

    public void OnStaminaChange(float oldValue, float newValue)
    {
        staminaSlider.value = playerStamina;        
    }

    public void CarriedObjectChange(GameObject oldValue, GameObject newValue)
    {

    }

 
    void Update()
    {
        if (isServer)
        {
            if (pCarry.carriedObject != null)
            {
                playerStamina -= Time.deltaTime;
            }
            if (pCarry.carriedObject == null && playerStamina <100)
            {
                playerStamina += Time.deltaTime/5;
            }
        }

    }
}
