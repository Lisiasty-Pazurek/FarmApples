
using System.Collections.Generic;
using UnityEngine;
using MirrorBasics;

public class Checkpoint : MonoBehaviour
{
    public int id;
    public LevelController levelController;

    public void Start() 
    {
    }

    private void OnTriggerEnter(Collider other) 
    {    
        if (other.gameObject.GetComponent<Runner>() == null) {return;}
        
        else if (other.gameObject.GetComponent<Runner>() != null)
        {
            levelController.CheckifPlayersFinished();            
            if (other.gameObject.GetComponent<Runner>().visitedCheckpoints.ContainsKey(this.id - 1))
            other.gameObject.GetComponent<Runner>().VisitCheckpoint(id,levelController.gameTimer);
        }

        

        if (other.gameObject.GetComponent<Carrier>() != null)
        {
            
        }
    }
}
