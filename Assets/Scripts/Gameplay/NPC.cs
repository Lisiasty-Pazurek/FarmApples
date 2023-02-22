using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SphereCollider))]
public class NPC : MonoBehaviour
{
    public DialogueSystem dialogueNPC;
    // Start is called before the first frame update
    void Start()
    {
        dialogueNPC = GetComponent<DialogueSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay(Collider other) 
    {
        if (other.tag == "Player")
        {
            Debug.Log("Player in range, he can interact and display dialogue window");
            if (Input.GetButtonDown("F"))
            {
                StartDialogue();
            }
        }
    }

    public void StartDialogue()
    {
        dialogueNPC.StartDialogue();
    }

}
