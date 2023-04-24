using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using MirrorBasics;

public class Cauldron : NetworkBehaviour
{
    public int teamID;
    public Recipe recipe;
    public List<Recipe> recipeList = new List<Recipe>();

    public void Update() 
    {
        if (recipe.ingredientsList.Count == 0)
        {
            // Objective done
        }
    }

    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerScore>().teamID == this.teamID )
        {
            if (other.gameObject.GetComponent<Carrier>().carriedObject != null)
            {
                foreach (Ingredient item in recipe.ingredientsList)
                {
                    if (item.ingredientName == other.gameObject.GetComponent<Carrier>().carriedObject.GetComponent<Ingredient>().ingredientName)
                    {
                        recipe.ingredientsList.Remove(other.gameObject.GetComponent<Carrier>().carriedObject.GetComponent<Ingredient>());
                        other.gameObject.GetComponent<Carrier>().carriedObject = null;
                        
                    }
                }
            }
        }
    }
}
