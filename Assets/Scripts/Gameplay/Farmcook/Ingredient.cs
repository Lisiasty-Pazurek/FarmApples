using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Ingredient : NetworkBehaviour
{
    public bool isCarried;
    public string ingredientName;
    private Collider ingredientCollider;
    public List<GameObject> ingredientType = new List<GameObject>();

    public override void OnStartServer()
    {
        ingredientCollider = GetComponent<SphereCollider>();
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
        this.transform.SetParent(player.GetComponent<Cook>().rootTransform); 
        isCarried = true;
        ingredientCollider.enabled = false;
    }

    [ServerCallback]
    public void DropItem(GameObject player)
    {
        player.GetComponent<Cook>().carriedObject = null;
        this.transform.SetParent(null); 
        isCarried = false;
        ingredientCollider.enabled = true;
    }

    
}
