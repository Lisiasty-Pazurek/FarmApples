using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;


public class PlayerStamina : NetworkBehaviour
{
    [SyncVar (hook =nameof(OnStaminaChange))]public float playerStamina;

    [SerializeField] private float maxStamina;
    public Carrier pCarry;
    public Transform rootTransform;
    [SerializeField] private Slider staminaSlider;

    public void Start() 
    {
        if (pCarry ==null) {pCarry = this.gameObject.GetComponent<Carrier>();}
    }

    public void OnStaminaChange(float oldValue, float newValue)
    {
        staminaSlider.value = playerStamina;        
    }

 
    void Update()
    {
        if (isServer)
        {
            if (pCarry.carriedObject != "")
            {
                playerStamina -= Time.deltaTime;
            }
            if (pCarry.carriedObject == "" && playerStamina <100)
            {
                playerStamina += Time.deltaTime/5;
            }

            if (playerStamina >= maxStamina)
            {
                // Invoke max stamina event
            }

            if (playerStamina <= 0)
            {
                // Invoke empty stamina event                    
            }
        }

    }

    void ChangeStamina(float value)
    {
        playerStamina -= value;
    }
}
