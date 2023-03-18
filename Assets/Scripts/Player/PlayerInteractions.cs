using UnityEngine;
using DialogueSystem;
using UnityEngine.UI;
using MirrorBasics;

public class PlayerInteractions : MonoBehaviour
{

     void OnTriggerStay(Collider other) 
    {
        if (!this.gameObject.GetComponent<PlayerController>().isLocalPlayer) {return;}
        if (other.GetComponent<DialogueInteract>() !=null )
        {
            FindObjectOfType<UIGameplay>().interactImage.enabled = true;            
            if (Input.GetKeyDown(KeyCode.F))
            {
                other.GetComponent<DialogueInteract>().StartDialogue();
            }
        }
    }

    private void OnTriggerEnter(Collider other)     {    }
    private void OnTriggerExit(Collider other) 
    {
        if (!this.gameObject.GetComponent<PlayerController>().isLocalPlayer) {return;}
        FindObjectOfType<UIGameplay>().interactImage.enabled = false;
    }
    

}
