using UnityEngine;
using DialogueSystem;
using UnityEngine.UI;
using MirrorBasics;

public class PlayerInteractions : MonoBehaviour
{
    public bool canInteract = false;
    public UIGameplay uiGameplay;

    private void OnValidate() {

        uiGameplay = FindObjectOfType<UIGameplay>();

    }

     void OnTriggerStay(Collider other) 
    {
        if (!this.gameObject.GetComponent<PlayerController>().isLocalPlayer || !canInteract) {return;}
        if (other.GetComponent<DialogueInteract>() !=null )
        {
            FindObjectOfType<UIGameplay>().interactImage.enabled = true;            
            if (Input.GetKeyDown(KeyCode.F))
            {
                other.GetComponent<DialogueInteract>().StartDialogue();
            }
        }
    }

    private void OnTriggerEnter(Collider other)     
    { 
        if (!this.gameObject.GetComponent<PlayerController>().isLocalPlayer || !canInteract) {return;}   
    }
    private void OnTriggerExit(Collider other) 
    {
        if (!this.gameObject.GetComponent<PlayerController>().isLocalPlayer || !canInteract) {return;}
        if (FindObjectOfType<UIGameplay>().interactImage != null) {FindObjectOfType<UIGameplay>().interactImage.enabled = false;}
    }
    

}
