using UnityEngine;
using Mirror;


public class Pickup : NetworkBehaviour
{
    [SyncVar (hook = nameof(HandlePickupNameChange))]public string pickupName;
    public int beenCarriedBy;
    public Ingredient ingredient;

    public override void OnStartServer()
    {
        base.OnStartServer();
        RpcPickupModel(pickupName);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        SetThisModel(pickupName);
    }

    [Server]
    void HandlePickupNameChange(string oldValue, string newValue)
    {
        if (isServer)
        {
            RpcPickupModel(newValue);
        }

        // if (isClient)
        // {
        //     SetThisModel(newValue);
        // }

    }


    [ClientRpc]
    public void RpcPickupModel(string ingName)
    {   
        SetThisModel(ingName);
    }

    public void SetThisModel(string ingName)
    {
        print("set: " + ingName);
        if (ingredient != null)
        {
            foreach (GameObject ingredient in ingredient.ingredientObjects)
            {
                if (ingredient.name == ingName)
                {
                    ingredient.SetActive(true);
                    print("set: " + ingName);
                }
                else ingredient.SetActive(false);
            }
        }        
    }

    [ClientRpc]
    public void RpcPickupPosition(Vector3 location)
    {
        this.transform.position = location;
    }

}
