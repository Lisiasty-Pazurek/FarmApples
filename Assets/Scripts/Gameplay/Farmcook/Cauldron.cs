using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using MirrorBasics;

public class Cauldron : NetworkBehaviour
{
    [SyncVar][SerializeField]private int teamID;
    public Recipe recipe;
    public readonly SyncList<string> ingredientList = new SyncList<string>();
    public SyncList<string> currentIngredientList = new SyncList<string>();
    public List<Recipe> recipeList = new List<Recipe>();

    public void Update() 
    {

    }

    public override void OnStartServer()
    {
        int x = Random.Range(0,recipeList.Count);
        RpcRecipe(x);
        recipe = recipeList[x];
        ingredientList.AddRange(recipe.ingredientsList);
        currentIngredientList = ingredientList;
        base.OnStartServer();
    }

    [ClientRpc]
    void RpcRecipe(int sRecipe)
    {
        recipe = recipeList[sRecipe];
    
    }

    [Server]
    void AddItemToRecipe()
    {
        
    }



    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerScore>().teamID == this.teamID )
        {
            if (other.gameObject.GetComponent<Carrier>().carriedObject != null)
            {               
                foreach (string item in currentIngredientList)
                {
                    if (item == other.gameObject.GetComponent<Carrier>().carriedObject)
                    {
                        currentIngredientList.Remove(other.gameObject.GetComponent<Carrier>().carriedObject);
                        other.gameObject.GetComponent<Carrier>().RpcRemoveItem();
                    }
                }
            }
        }
    }

}
