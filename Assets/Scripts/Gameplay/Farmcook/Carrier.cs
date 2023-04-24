using System.Collections;
using System.Collections.Generic;
using MirrorBasics;
using UnityEngine;
using Mirror;

public class Carrier : NetworkBehaviour
{
    public bool isCarrying = false;
    public GameObject carriedObject;    
    public List<GameObject> itemsToCarry = new List<GameObject>();

    void Update()
    {
        if (isClient)
        {
            if (Input.GetKey(KeyCode.F))
            {
                if (carriedObject != null)
                {
                    SpawnDroppedItem();    
                }
            }
        }
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (itemsToCarry.Contains(other.gameObject)  && !isCarrying)
        {
            carriedObject = other.gameObject;
            Destroy(other.gameObject);
        }
    }

    [Command]
    public void SpawnDroppedItem()
    {
        GameObject spawnedObject = Instantiate(carriedObject, this.gameObject.transform);
        
    }

}
