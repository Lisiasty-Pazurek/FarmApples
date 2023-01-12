using UnityEngine;
using Mirror;

namespace MirrorBasics
{
    public class PlayerScore : NetworkBehaviour
    {
        [SyncVar]
        public int index;
        [SyncVar]
        public int teamID = 1;

        [SyncVar (hook = nameof(HandlePlayerScoreChange))]
        public int score;

        [SyncVar (hook = nameof(HandleCarriedItemToggle))]
        public bool hasItem;

        [SerializeField]
        public GameObject carriedItem = null;

        UIScore uiScore;

        void HandleCarriedItemToggle(bool oldValue, bool newValue)
        {   
            carriedItem.SetActive(newValue);
        }

        void HandlePlayerScoreChange (int oldValue, int newValue)
        {
            score = newValue;
   //        uiScore.SetPlayerScore(this);
        }
        // void OnGUI()
        // {
        //     GUI.Box(new Rect(10f + (index * 110), 10f, 100f, 25f), $"P{index}: {score:0000000}");
        // }

    }
}