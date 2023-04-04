
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
            if (other.gameObject.GetComponent<Runner>().visitedCheckpoints.ContainsKey(this.id - 1))
            other.gameObject.GetComponent<Runner>().VisitCheckpoint(id,levelController.gameTimer);

            if (other.gameObject.GetComponent<Runner>().visitedCheckpoints.ContainsKey(19))
            {
                levelController.CheckifPlayersFinished();
                Debug.Log("Player ended race, sending info to level controller to check if everyone finished it");
            }
        }

        

        if (other.gameObject.GetComponent<Carrier>() != null)
        {
            
        }
    }
}
