using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    [SerializeField] public Dialogue[] dialogues;
    public int currentDialogue = 0;


    void Start()
    {

    }

    public void StartDialogue()
    {
        StartCoroutine( DisplayDialogue());        
    }

    IEnumerator DisplayDialogue()
    {
        while (currentDialogue < dialogues.Length)
        {
            string[] options = dialogues[currentDialogue].options;
            string dialogue = dialogues[currentDialogue].dialogue;

            // Display dialogue text
            Debug.Log(dialogue);

            // Display options if there are any
            if (options.Length > 0)
            {
                for (int i = 0; i < options.Length; i++)
                {
                    Debug.Log((i + 1) + ": " + options[i]);
                }

                // Wait for player input
                int input = -1;
                while (input < 1 || input > options.Length)
                {
                    input = int.Parse(Input.inputString);
                }

                // Branch based on player input
                currentDialogue = dialogues[currentDialogue].branches[input - 1];
            }
            else
            {
                // No options, move to next dialogue
                currentDialogue++;
            }

            // Wait for a frame before continuing
            yield return null;
        }
    }
}

[System.Serializable]
public class Dialogue
{
    public string dialogue;
    public string[] options;
    public int[] branches;
}
