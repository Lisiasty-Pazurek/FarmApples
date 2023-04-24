using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Ingredient : NetworkBehaviour
{
    [SyncVar(hook =nameof(OnCarryChange))] public bool isCarried;

    public string ingredientName;
    private Collider ingredientCollider;

    private GameObject carryingPlayer;
    private Vector3 carryingPlayerPosition;
    public List<GameObject> ingredientType = new List<GameObject>();
    public int[] beenCarriedBy;

    public override void OnStartServer()
    {
        ingredientCollider = GetComponent<SphereCollider>();
    }

    [ServerCallback]
    public void Update ()
    {
        if (isCarried)
        {
            carryingPlayerPosition = carryingPlayer.GetComponent<Cook>().rootTransform.position;           
            this.transform.position = carryingPlayerPosition;            
        }
    }

// Server check if player can pick up reward
    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.GetComponent<Cook>().carriedObject != null) {return;}
            else
            PickUpItem(other.gameObject);               
        }
    }

// Server adds reward to a player and destroy pickup
    [ServerCallback]
    public void PickUpItem(GameObject player)
    {
        player.GetComponent<Cook>().carriedObject = this.gameObject;
        carryingPlayer = player;
        isCarried = true;
        ingredientCollider.enabled = false;
    }

    [Command (requiresAuthority =false)]
    public void DropItem(GameObject player)
    {
        player.GetComponent<Cook>().carriedObject = null;
        carryingPlayer = null;
        isCarried = false;
        ingredientCollider.enabled = true;
    }

    public void OnCarryChange(bool oldValue, bool newValue)
    {
        if (!isCarried)
        {
            
        }
    }

    
}
