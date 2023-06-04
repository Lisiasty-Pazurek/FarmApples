using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIReputationSystem : MonoBehaviour
{
    [SerializeField] private List<GameObject> reputationImages = new List<GameObject>();

    public void ReloadIcon(string playerRep)
    {
        foreach (GameObject image in reputationImages)
        {
            if (image.name == playerRep)
            {
                image.SetActive(true);
            }
            
        }
    }
}
