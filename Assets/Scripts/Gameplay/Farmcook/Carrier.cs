using System.Collections;
using System.Collections.Generic;
using MirrorBasics;
using UnityEngine;
using Mirror;
using UnityEngine.Animations;

public class Carrier : NetworkBehaviour
{
    [SyncVar] public bool isCarrying = false;
    [SyncVar (hook=nameof(HandleWeightChange))] public float weight;
    
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
        if (isServer)
        {
            if (isCarrying) {weight = 4;}
            else weight = 0;
        }
    }

    [Server]
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

    [TargetRpc]
    void SetUIIcon(string carriedItem)
    {
            if (carriedItem == "")
            {
                UIGameplay.instance.interactImage.sprite = UICook.instance.veggieImage[0];
            }            
            else
            foreach (Sprite sprite in UICook.instance.veggieImage)
            {
                if (sprite.name == carriedItem)
                UIGameplay.instance.interactImage.sprite = sprite;
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
                    GameObject carriedItemInSlot = Instantiate(item, carrySlot.transform.position, carrySlot.transform.rotation) ;
                    ConstraintSource itemSlot = new ConstraintSource();
                    itemSlot.sourceTransform = carrySlot.transform;
                    carriedItemInSlot.GetComponent<ParentConstraint>().AddSource(itemSlot);
                }      
                spawningItem = false;
            }
        }
    }

    [Command (requiresAuthority = false)]
    public void CmdDropItem()
    {
        if (carriedObject != "")
        {

            isCarrying = false;

            RpcPlayerDropPrefab();
            if (!spawningItem) { SpawnDroppedItem();  } 
        }
    }

    [ClientRpc]
    public void RpcPlayerDropPrefab()
    {
        Destroy(carrySlot.transform.GetChild(carrySlot.transform.childCount-1).gameObject);             
    }
    

    [Server]
    public void HandleCarriedItemChange(string oldValue, string newValue)
    {   
        RpcCarriedItemChange (newValue);
        SetUIIcon(newValue);
    
    }

    [ClientRpc]
    private void RpcCarriedItemChange (string carrieditem)
    {
        foreach (GameObject item in carriedItems)
        {
            if (item.name == carrieditem)
            {
                Instantiate(item,carrySlot.transform) ;
                print("spawned prefab on player");
            }
        }  
    }

    [Server]
    void HandleWeightChange(float oldValue, float newValue)
    {

    }

    [ClientRpc]
    public void RpcRemoveItem ()
    {
        carriedObject = ""; 
        isCarrying = false;
        RpcPlayerDropPrefab();
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
            spawnedObject.GetComponent<Pickup>().pickupName = carriedObject;
            spawnedObject.GetComponent<Ingredient>().ingredientName = carriedObject;
            Vector3 location = new Vector3(this.gameObject.transform.position.x,this.gameObject.transform.position.y, this.gameObject.transform.position.z);
            spawnedObject.GetComponent<Pickup>().RpcPickupPosition(location);
            //spawnedObject.transform.SetParent(Scene);
            spawningItem = false;            
            carriedObject = "";  
            isCarrying = false;          
            return;
        }

    }

}
