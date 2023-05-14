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
    private PlayerController pController;
    public Transform rootTransform;

    [SerializeField] private Slider staminaSlider;

    public void Start() 
    {
        pController = GetComponent<PlayerController>();
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
            ChangeStaminaMod();
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
    void ChangeStaminaMod()
    {
        if (pCarry != null)
        {
            if (pCarry.weight != 0) {staminaMod = - pCarry.weight;}     
            else if (!GetComponent<Rigidbody>().IsSleeping())
            {
                staminaMod = staminaRegen;  
            }
            else if (GetComponent<Rigidbody>().IsSleeping())
            {
                staminaMod = staminaRegen *2;
            }
        }
    }
}
