using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;


public class PlayerStamina : NetworkBehaviour
{
    [SyncVar (hook =nameof(OnStaminaChange))]public float playerStamina;
    [SerializeField]private float staminaRegen = 1;     
    [SerializeField][SyncVar]private float staminaMod = 0;

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
            ChangeStamina();
            playerStamina += Time.deltaTime * staminaMod;

            if (playerStamina >= maxStamina)
            {
                // Invoke max stamina event
                playerStamina = maxStamina;
            }

            if (playerStamina <= 0)
            {
                // Invoke empty stamina event              
                if (pCarry != null)
                {
                    pCarry.CmdDropItem();
                }     
                playerStamina = 0;
            }
        }

    }

    [Server]
    void ChangeStamina()
    {
        if (pCarry != null)
        {
            if (pCarry.weight != 0) {staminaMod = - pCarry.weight;}
            else staminaMod = staminaRegen;        
        }
    }
}
