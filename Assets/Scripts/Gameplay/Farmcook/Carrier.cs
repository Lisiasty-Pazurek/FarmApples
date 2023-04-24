using System.Collections;
using System.Collections.Generic;
using MirrorBasics;
using UnityEngine;
using Mirror;

public class Carrier : NetworkBehaviour
{
    [SyncVar ]public bool isCarrying = false;
    
 //   public List<string> itemsToCarry = new List<string>(); // not used for now, gonna use it for checking other object
    public List<GameObject> carriedItems = new List<GameObject>();    
    public GameObject carrySlot; // object to spawn carried item
    [SyncVar (hook=nameof(HandleCarriedItemChange))] public string carriedObject;  // name of carried object

    public GameObject pickupPrefab;
    public bool spawningItem;


    private void Start() 
    {
        carrySlot = this.gameObject.GetComponentInChildren<PlayerModel>().rootTransform;

        
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                if (carriedObject != "")
                {
                    CmdDropItem();
                }
                else return;
            }
        }
        
    }

[ServerCallback]
    private void OnTriggerEnter(Collider other) 
    {

        if (other.gameObject.GetComponent<Pickup>() != null && other.gameObject.GetComponent<Ingredient>() != null && !isCarrying) //&& itemsToCarry.Contains(other.gameObject.name.ToString())  <not working - why?
        {
            print("item on list");

            if (other.gameObject.GetComponent<Ingredient>() != null 
                && other.gameObject.GetComponent<Pickup>().beenCarriedBy != this.gameObject.GetComponent<PlayerController>().playerIndex)
            {
                foreach (GameObject carriedItem in carriedItems)
                {
                    if (carriedItem.name == other.gameObject.GetComponent<Ingredient>().ingredientName)
                    {
                        CmdPickupItem(carriedItem.name, other.gameObject); 
                    }
                }
            }    
        }
    }

    [Command (requiresAuthority = false)]
    void CmdPickupItem(string carriedItem, GameObject pickupItem) 
    {
        carriedObject = pickupItem.gameObject.GetComponent<Ingredient>().ingredientName;
        isCarrying = true;
        print("command sent for " + carriedItem);
        RpcPlayerPickPrefab(carriedObject);        
        NetworkServer.Destroy(pickupItem);   
    }

    [ClientRpc]
    void RpcPlayerPickPrefab(string carriedObjectName)
    {
        foreach (GameObject item in carriedItems)
        {
            if (item.name == carriedObjectName)
            {
                if (carrySlot.transform.childCount == 0 && !spawningItem)
                {
                    spawningItem = true;
                    GameObject carriedItemInSlot = Instantiate(item,carrySlot.transform) ;
                }      
                spawningItem = false;
            }
        }
    }

    [Command (requiresAuthority = false)]
    void CmdDropItem()
    {
        isCarrying = false;
        carriedObject = "";
        RpcPlayerDropPrefab();
        if (!spawningItem) { SpawnDroppedItem();  } 
    }

    [ClientRpc]
    void RpcPlayerDropPrefab()
    {
        Destroy(carrySlot.transform.GetChild(carrySlot.transform.childCount-1).gameObject);             
    }
    
    public void HandleCarriedItemChange(string oldValue, string newValue)
    {
        foreach (GameObject item in carriedItems)
        {
            if (item.name == newValue)
            {
                Instantiate(item,carrySlot.transform) ;
                print("spawned prefab on player");
            }
        }    
    }

    [Server]
    public void SpawnDroppedItem()
    {
        spawningItem = true;
        if (carriedObject != null)
        {
            GameObject spawnedObject = Instantiate(pickupPrefab);
            NetworkServer.Spawn(spawnedObject);
            spawnedObject.transform.position = this.gameObject.transform.position;
            spawnedObject.GetComponent<Pickup>().beenCarriedBy = this.gameObject.GetComponent<PlayerController>().playerIndex;
            spawnedObject.GetComponent<Ingredient>().ingredientName = carriedObject;
            Vector3 location = new Vector3(this.gameObject.transform.position.x,this.gameObject.transform.position.y, this.gameObject.transform.position.z);
            spawnedObject.GetComponent<Pickup>().RpcPickupPosition(location);
            //spawnedObject.transform.SetParent(Scene);
            spawningItem = false;            
            return;
        }
    }

}