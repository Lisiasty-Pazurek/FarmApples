using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using MirrorBasics;

public class Cauldron : NetworkBehaviour
{
    public int teamID;
    public Recipe recipe;
    public SyncList<string> ingredientList = new SyncList<string>();
    public List<Recipe> recipeList = new List<Recipe>();

    public void Update() 
    {
        if (recipe.ingredientsList.Count == 0)
        {
            // Objective done
        }
    }

    public override void OnStartServer()
    {
        int x = Random.Range(0,recipeList.Count);
        RpcRecipe(x);
        recipe = recipeList[x];
        ingredientList.AddRange(recipe.ingredientsList);
        base.OnStartServer();
    }
    [ClientRpc]
    void RpcRecipe(int sRecipe)
    {
        recipe = recipeList[sRecipe];
    
    }


    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerScore>().teamID == this.teamID )
        {
            if (other.gameObject.GetComponent<Carrier>().carriedObject != null)
            {
                foreach (string item in recipe.ingredientsList)
                {
                    if (item == other.gameObject.GetComponent<Carrier>().carriedObject)
                    {
                        recipe.ingredientsList.Remove(other.gameObject.GetComponent<Carrier>().carriedObject);
                        other.gameObject.GetComponent<Carrier>().carriedObject = "";

                    }
                }
            }
        }
    }
}
