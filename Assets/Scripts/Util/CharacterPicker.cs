using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPicker : MonoBehaviour
{
    public Dictionary <string, Sprite> characters = new Dictionary<string, Sprite>();
    public List<Sprite> modelSprites = new List<Sprite>();

    public void Start() 
    {
        characters.Add("Frog", (Sprite)Resources.Load("Assets/GFX/Sprite/Frog.jpg"));
        characters.Add("ArcticFox", (Sprite)Resources.Load("Assets/GFX/Sprite/ArcticFox.jpg"));
    }

}
