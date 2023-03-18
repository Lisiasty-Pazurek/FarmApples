using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour
{
    public Image interactImage;
     void OnTriggerStay(Collider other) 
    {
        if (other.GetComponent<DialogueInteract>() !=null )
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                other.GetComponent<DialogueInteract>().StartDialogue();
            }
        }
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<DialogueInteract>() !=null )
        {
            interactImage.enabled = true;
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if (other.GetComponent<DialogueInteract>() !=null )
        {
            interactImage.enabled = false;
        }
    }
    

}
