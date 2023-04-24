using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using MirrorBasics;

public class Cauldron : NetworkBehaviour
{
    public int teamID;
    public Recipe recipe;
    public List<string> recipeList = new List<string>();

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
                foreach (string item in recipe.ingredientsList)
                {
                    if (item == other.gameObject.GetComponent<Carrier>().carriedObject)
                    {
                        recipe.ingredientsList.Remove(other.gameObject.GetComponent<Carrier>().carriedObject);
                        other.gameObject.GetComponent<Carrier>().carriedObject = null;

                    }
                }
            }
        }
    }
}
