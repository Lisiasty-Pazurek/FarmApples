using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "New Recipe", menuName = "Recipe")]
public class Recipe : MonoBehaviour
{
    public readonly string recipeName;
    public List<string> ingredientsList = new List<string>(); // link prefabs here, maybe it's gonna be better to change it to string list for names

}
