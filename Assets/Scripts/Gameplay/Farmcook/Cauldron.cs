using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using MirrorBasics;
using UnityEngine.Events;


public class Cauldron : NetworkBehaviour
{
    public UnityAction OnRecipeChange;
    [SyncVar][SerializeField]public int teamID ;
    public Recipe recipe;
    [SyncVar] public string recipeName;
    public readonly List<string> ingredientList = new List<string>();
    public readonly SyncList<string> currentIngredientList = new SyncList<string>();
    public List<Recipe> recipeList = new List<Recipe>();

    public void Update() 
    {

    }

    private void Start() 
    {
        UICook.instance.cauldronList.Add(this);
    }
    public override void OnStartServer()
    {
        int x = Random.Range(0,recipeList.Count);
        recipe = recipeList[x];
        recipeName = recipe.name;       
        ingredientList.AddRange(recipe.ingredientsList);
        currentIngredientList.AddRange(ingredientList);
        RpcCurrentRecipeToTeam();
        base.OnStartServer();
    }

    public override void OnStartClient()
    {
        print ("setting stuff for local player on cauldron ####");
       
        foreach (Recipe item in recipeList)
        {
            if (item.name == recipeName)
            {
                recipe = item;
            }

        }
        // FindAnyObjectByType<UICook>().SetCauldron(this);            
        UICook.instance.SetCauldron();
    }

    [ClientRpc]
    void RpcRecipe(int sRecipe)
    {
        recipe = recipeList[sRecipe];
    }

    [ClientRpc]
    void RpcCurrentRecipeToTeam()
    {
        if (LevelController.singleton.pController.GetComponent<PlayerScore>().teamID == teamID)
        {
            OnRecipeChangeEvent();
        }
    }

  
    void OnRecipeChangeEvent ()
    {
        OnRecipeChange?.Invoke();        
        print("invoking event");
    }


    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerScore>().teamID == this.teamID )
        {
            print("hit");
            if (other.gameObject.GetComponent<Carrier>().carriedObject != null)
            {               
                if (currentIngredientList.Contains(other.gameObject.GetComponent<Carrier>().carriedObject))

                {
                    currentIngredientList.Remove(other.gameObject.GetComponent<Carrier>().carriedObject);
                    other.gameObject.GetComponent<Carrier>().RpcRemoveItem();
                    RpcCurrentRecipeToTeam();
                }
            }
        }
        if (currentIngredientList.Count == 0) {LevelController.singleton.EndLevel(teamID);}        
    }

}
