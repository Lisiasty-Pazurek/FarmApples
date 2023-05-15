using UnityEngine;
using Mirror;

namespace MirrorBasics
{
    public class PlayerScore : NetworkBehaviour
    {
   
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

    [ServerCallback]
    void OnTriggerEnter(Collider other)
    {
        // Stealing ability 
        
        if (other.gameObject.CompareTag("Player") && teamID != other.GetComponent<PlayerScore>().teamID)
        {
            if (!canSteal){return;}
            if (other.gameObject.GetComponent<PlayerScore>().hasItem && !hasItem) 
            {                
                other.gameObject.GetComponent<PlayerScore>().hasItem = false;
                hasItem = true;
                canSteal = false;
            }
            else return;
        }           
    }

    }
}