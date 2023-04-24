using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Ingredient : MonoBehaviour
{
    public string ingredientName;
    public int beenCarriedBy;
    public Dictionary<string,GameObject> ingredientType = new Dictionary<string, GameObject>();

    
}
