﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueSystem {

    [CreateAssetMenu (fileName = "DialogueObject", menuName = "NPC Dialogue Object", order = 0)]
    public class DialogueObject : ScriptableObject {

        [Header ("Dialogue")]
        public List<DialogueSegment> dialogueSegments = new List<DialogueSegment> ();

    }

    [System.Serializable]
    public struct DialogueSegment {
        public string dialogueText;
        public float dialogueDisplayTime;
        public string dialogueReward;
        public string dialougueCost;
        public List<DialogueChoice> dialogueChoices;

    }

    [System.Serializable]
    public struct DialogueChoice {
        public string dialogueChoice;
        public string dialogueRequirement;      
        public DialogueObject followOnDialogue;
    }

}