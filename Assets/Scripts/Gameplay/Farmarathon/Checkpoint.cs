using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirrorBasics;

public class Checkpoint : MonoBehaviour
{
    public int id;
    public LevelController levelController;

    public void Start() 
    {
        levelController = FindObjectOfType<LevelController>();


        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<Carrier>() != null)
        {
            
        }
        
        if (other.GetComponent<Runner>() != null)
        {
            other.GetComponent<Runner>().visitedCheckpoints.Add(this, levelController.gameTimer); 
        }


    }
}
