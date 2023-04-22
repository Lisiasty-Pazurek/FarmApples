using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Ingredient : NetworkBehaviour
{
    public bool isCarried;
    public string ingredientName;


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
        this.transform.SetParent(player.transform); 
        isCarried = true;
    }
    
}
