using UnityEngine;
using Mirror;

namespace MirrorBasics
{
    public class PlayerScore : NetworkBehaviour
    {
        [SyncVar] public int index;
        [SyncVar] public int teamID = 1;

        [SyncVar (hook = nameof(HandlePlayerScoreChange))]  public int score;

        [SyncVar (hook = nameof(HandleCarriedItemToggle))] public bool hasItem;
        [SyncVar (hook = nameof(HandleStealingToggle))] public bool canSteal = false;
        [SyncVar] public bool isNavigator;

        [SerializeField] public GameObject carriedItem;
        [SerializeField] public GameObject stealingItem;

        [SerializeField] public UIScore uiScore;


        public override void OnStartLocalPlayer()
        {
            uiScore = GameObject.FindObjectOfType<UIScore>();      
            uiScore.player = this;
            uiScore.SetPlayerName();
        }
        public override void OnStartServer()
        {

        }

        void HandleCarriedItemToggle(bool oldValue, bool newValue)
        {   
            carriedItem.SetActive(newValue);
            if (isLocalPlayer) {uiScore.SetStatusIcon("apple", newValue);}
        }

        void HandleStealingToggle(bool oldValue, bool newValue)
        {   
            stealingItem.SetActive(newValue);
            if (isLocalPlayer) {uiScore.SetStatusIcon("stealing", newValue);}
        }


        void HandlePlayerScoreChange (int oldValue, int newValue)
        {
            if (!isLocalPlayer) return;
            else
            uiScore = GameObject.FindObjectOfType<UIScore>();   
            uiScore.SetPlayerScore(score);
        }

        // void HandleTeamScoreChange (int teamboxID, int teamPoints)
        // {
        //     if (!isLocalPlayer) return;
        //     else
        //     uiScore.SetTeamScore(teamboxID, teamPoints);
        // }

    }
}