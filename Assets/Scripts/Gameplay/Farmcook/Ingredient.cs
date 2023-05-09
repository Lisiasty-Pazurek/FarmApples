using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ingredient : MonoBehaviour
{
    public string ingredientName;

    public GameObject objectType;
    public Dictionary<string,GameObject> ingredientType = new Dictionary<string, GameObject>();
    public List<GameObject> ingredientObjects;

    void Start() 
    {
        ingredientType.Add(ingredientName,objectType);
    }

    void IngredientType()
    {
        ingredientType.TryGetValue(ingredientName,out objectType);
    }
}
