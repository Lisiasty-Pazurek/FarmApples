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
//      [SyncVar]
        public int score;

        [SyncVar (hook = nameof(HandleCarriedItemToggle))]   
        public bool hasItem;

        [SerializeField]
        public GameObject carriedItem;

        [SerializeField]
        public UIScore uiScore;

        public override void OnStartLocalPlayer()
        {
            
            uiScore = GameObject.FindObjectOfType<UIScore>();      
            uiScore.player = this;
//            uiScore.SetPlayerName(Player.localPlayer);
        }


        void HandleCarriedItemToggle(bool oldValue, bool newValue)
        {   
            carriedItem.SetActive(newValue);
        }

        [Client]
        void HandlePlayerScoreChange (int oldValue, int newValue)
        {
            uiScore = GameObject.FindObjectOfType<UIScore>();   
            uiScore.SetPlayerScore(score);
        }

    }
}