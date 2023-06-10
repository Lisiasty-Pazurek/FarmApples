using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace DialogueSystem {
    public class DialogueInteract : MonoBehaviour {

        [SerializeField] public Canvas dialogueCanvas;
        [SerializeField] Text dialogueText;
        [SerializeField] GameObject dialogueOptionsContainer;
        [SerializeField] Transform dialogueOptionsParent;
        [SerializeField] GameObject dialogueOptionsButtonPrefab;
        [SerializeField] DialogueObject startDialogueObject;
        [SerializeField] DialogueObject alternateDialogueObject;

        public bool dialogueStarted {private set; get;}
        
        private List<GameObject> spawnedButtons;
        [SerializeField] public List<string> rewardedItems;
        public PlayerReputation activePlayer;


        bool optionSelected = false;


        public void StartDialogue () {
            StartCoroutine (DisplayDialogue (startDialogueObject));
        }

        public void StartDialogue (DialogueObject _dialogueObject) {
            StartCoroutine (DisplayDialogue (_dialogueObject));
        }

        public void StartDialogue (PlayerReputation playerReputation) {
            activePlayer = playerReputation;
            
            if (rewardedItems.Intersect(activePlayer.reputation).Any()) 
            {
                StartCoroutine (DisplayDialogue (alternateDialogueObject));
            }
            else 
            StartCoroutine (DisplayDialogue (startDialogueObject));
            
        }

        public void OptionSelected (DialogueObject selectedOption) {
            optionSelected = true;
            StartDialogue (selectedOption);  
        }



        IEnumerator DisplayDialogue (DialogueObject _dialogueObject) {
            yield return null;
            Debug.Log ("Starting Dialogue Chain");
            if (!dialogueStarted)
            {
                spawnedButtons = new List<GameObject> ();
                dialogueStarted = true;
                dialogueCanvas.enabled = true;


                foreach (var dialogue in _dialogueObject.dialogueSegments) {
                    print("dialogue ping");
                    dialogueText.text = dialogue.dialogueText;
                    if (dialogue.dialogueReward != "" && !activePlayer.reputation.Contains(dialogue.dialogueReward))
                    {
                        activePlayer.AddReputation(dialogue.dialogueReward);
                    }
                    if (dialogue.dialougueCost != "" && activePlayer.reputation.Contains(dialogue.dialogueReward))
                    {
                        activePlayer.RemoveReputation(dialogue.dialougueCost);
                    }

                    if (dialogue.dialogueChoices.Count == 0) 
                    {
                        yield return new WaitForSeconds (dialogue.dialogueDisplayTime);
                    } 
                    else 
                    {
                        dialogueOptionsContainer.SetActive (true);
                        //Open options panel
                        foreach (var option in dialogue.dialogueChoices) 
                        {
                            if (option.dialogueRequirement == "" || activePlayer.reputation.Contains(option.dialogueRequirement))
                            {
                                GameObject newButton = Instantiate (dialogueOptionsButtonPrefab, dialogueOptionsParent);
                                spawnedButtons.Add (newButton);
                                newButton.GetComponent<UIDialogueOption> ().Setup (this, option.followOnDialogue, option.dialogueChoice);
                            }
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
                dialogueStarted = false;

                spawnedButtons.ForEach (x => Destroy (x));
                Debug.Log ("Ending Dialogue Chain");
            }
            
        }

    }
}