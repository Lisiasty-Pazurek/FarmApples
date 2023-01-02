using UnityEngine;
using Mirror;

    public class PlayerScore : NetworkBehaviour
    {
        [SyncVar]
        public int index;

        [SyncVar]
        public uint score;

        [SyncVar]
        public int teamID = 1;

        [SyncVar (hook = nameof(HandleCarriedItemToggle))]
        public bool hasItem;

        [SerializeField]
        public GameObject carriedItem = null;

        void OnGUI()
        {
            GUI.Box(new Rect(10f + (index * 110), 10f, 100f, 25f), $"P{index}: {score:0000000}");
        }

        private void OnValidate() 
        {
            
        }

        void HandleCarriedItemToggle(bool oldValue, bool newValue)
        {   
            carriedItem.SetActive(newValue);
            
        }
    }

