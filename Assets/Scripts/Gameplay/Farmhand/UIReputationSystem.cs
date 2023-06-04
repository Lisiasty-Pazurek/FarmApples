using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReputationSystem : MonoBehaviour
{
    [SerializeField] private List<Image> reputationImages = new List<Image>();

    public void ReloadIcon(string playerRep)
    {
        foreach (Image image in reputationImages)
        {
            if (image.name == playerRep)
            {
                image.enabled = true;
            }
            
        }
    }
}
