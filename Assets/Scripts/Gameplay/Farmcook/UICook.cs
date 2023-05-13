using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MirrorBasics;

public class UICook : MonoBehaviour
{
    public static UICook instance {get; private set; }
    [SerializeField] private Transform recipeTransform;
    [SerializeField] private GameObject ingredientTemplate;
    public UIGameplay uIGameplay;
    public Cauldron cauldron;
    public PlayerController pController;

    public List<Cauldron> cauldronList = new List<Cauldron>();
    void Awake()
    {
        // add listeners to event here
        uIGameplay = GetComponent<UIGameplay>();
        instance = this;
        
    }

    private void  Start() 
    {
        pController = uIGameplay.player;
    }

    public void SetCauldron()
    { 
        foreach ( Cauldron cldr in cauldronList)
        {
            if (uIGameplay.player.GetComponent<PlayerScore>().teamID == cldr.teamID)
            {
                print("assigned cauldron of name for player: " + uIGameplay.player.name);            
                cauldron = cldr; 
                cauldron.OnRecipeChange += SpawnRecipe;
            }
        }
        SpawnRecipe();
    }
    

    void SpawnRecipe()
    {
        print("invoked event on UIcook");
        ClearIngredientList();

        foreach (string item in cauldron.currentIngredientList)
        {
            SpawnIngredient(item);
        }
    }

    void SpawnIngredient(string ingredientName)
    {
        GameObject ingredient = Instantiate(ingredientTemplate, recipeTransform);
        ingredient.GetComponentInChildren<Text>().text = ingredientName;
        ingredient.SetActive(true);
    }

    void ClearIngredientList()
    {
        foreach(Transform child in recipeTransform.transform)
        {
            Destroy(child.gameObject);
        }
    }
}
