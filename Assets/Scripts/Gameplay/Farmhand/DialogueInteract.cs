using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DialogueSystem {
    public class DialogueInteract : MonoBehaviour {

        [SerializeField] Canvas dialogueCanvas;
        [SerializeField] Text dialogueText;
        [SerializeField] GameObject dialogueOptionsContainer;
        [SerializeField] Transform dialogueOptionsParent;
        [SerializeField] GameObject dialogueOptionsButtonPrefab;
        [SerializeField] DialogueObject startDialogueObject;
        private List<GameObject> spawnedButtons;

        bool optionSelected = false;

        public void StartDialogue () {
            StartCoroutine (DisplayDialogue (startDialogueObject));
        }

        public void StartDialogue (DialogueObject _dialogueObject) {
            StartCoroutine (DisplayDialogue (_dialogueObject));
        }

        public void OptionSelected (DialogueObject selectedOption) {
            optionSelected = true;
            StartDialogue (selectedOption);
        }

        IEnumerator DisplayDialogue (DialogueObject _dialogueObject) {
            yield return null;
            Debug.Log ("Starting Dialogue Chain");
            spawnedButtons = new List<GameObject> ();

            dialogueCanvas.enabled = true;
            foreach (var dialogue in _dialogueObject.dialogueSegments) {
                print("dialogue ping");
                dialogueText.text = dialogue.dialogueText;

                if (dialogue.dialogueChoices.Count == 0) 
                {
                    yield return new WaitForSeconds (dialogue.dialogueDisplayTime);
                } 
                else 
                {
                    dialogueOptionsContainer.SetActive (true);
                    //Open options panel
                    foreach (var option in dialogue.dialogueChoices) {
                        GameObject newButton = Instantiate (dialogueOptionsButtonPrefab, dialogueOptionsParent);
                        spawnedButtons.Add (newButton);
                        newButton.GetComponent<UIDialogueOption> ().Setup (this, option.followOnDialogue, option.dialogueChoice);
                    }

                    while (!optionSelected) {
                        yield return null;
                    }
                    break;
                }
            }
            dialogueOptionsContainer.SetActive (false);
            dialogueCanvas.enabled = false;
            optionSelected = false;

            spawnedButtons.ForEach (x => Destroy (x));
            Debug.Log ("Ending Dialogue Chain");
        }

    }
}