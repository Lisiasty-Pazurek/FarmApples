using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Pickup : NetworkBehaviour
{
    public string pickupName;
    public int beenCarriedBy;
    public Ingredient ingredient;

    public void Start()
    {
        SetPickupModel();
    }

    public void SetPickupModel()
    {
        if (ingredient != null)
        {
            foreach (GameObject ingredient in ingredient.ingredientObjects)
            {
                if (ingredient.name == pickupName)
                {
                    ingredient.SetActive(true);
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
