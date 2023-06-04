using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReputationSystem : MonoBehaviour
{
    [SerializeField] private List<Image> reputationImages = new List<Image>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReloadIcon(string playerRep)
    {
        foreach (Image sprite in reputationImages)
        {
            if (sprite.name == playerRep)
            {
                sprite.enabled = true;
            }
            else 
            sprite.enabled = false;
        }
    }
}
