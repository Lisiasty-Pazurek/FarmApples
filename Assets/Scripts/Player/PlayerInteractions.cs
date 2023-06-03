using UnityEngine;
using DialogueSystem;
using UnityEngine.UI;
using MirrorBasics;

public class PlayerInteractions : MonoBehaviour
{
    public bool canInteract = true;
    public UIGameplay uiGameplay;
    [SerializeField] private PlayerController pController;

    private DialogueInteract interactableDialogue;

    private void OnValidate() {

        uiGameplay = UIGameplay.instance;
        pController = GetComponent<PlayerController>();
    }

    private void Update() 
    {
        if (!GetComponent<PlayerController>().isLocalPlayer){return;}
        else 
        if (Input.GetKeyUp(KeyCode.F))
        {
            DialogueInteract();
        }        
        
    }

    private void DialogueInteract()
    {
        
        interactableDialogue?.StartDialogue(this.GetComponent<PlayerReputation>());        
    }

    private void OnTriggerEnter(Collider other)     
    { 
        if (!this.gameObject.GetComponent<PlayerController>().isLocalPlayer || !canInteract) {return;}   
        if (GetComponent<PlayerController>().uiGameplay.interactImage != null && other.GetComponent<DialogueInteract>() != null) 
        {GetComponent<PlayerController>().uiGameplay.interactImage.enabled = true;}
        if (other.GetComponent<DialogueInteract>() != null)
        {
            interactableDialogue = other.GetComponent<DialogueInteract>();
        }
    }
    private void OnTriggerExit(Collider other) 
    {
        if (!this.gameObject.GetComponent<PlayerController>().isLocalPlayer || !canInteract) {return;}
        if (GetComponent<PlayerController>().uiGameplay.interactImage != null) 
        {GetComponent<PlayerController>().uiGameplay.interactImage.enabled = false;}
        interactableDialogue = null;
    }
    

}
