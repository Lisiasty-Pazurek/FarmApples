using UnityEngine;
using Mirror;

namespace MirrorBasics
{
    public class Pickup : NetworkBehaviour
    {
        public bool available = true;

        [SerializeField]  public string type;

        public override void OnStartServer ()
        {
            
        }

// Server check if player can pick up reward
        [ServerCallback]
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && this.GetComponent<NetworkMatch>().matchId == other.GetComponent<NetworkMatch>().matchId)
            {
                if (other.gameObject.GetComponent<PlayerScore>().hasItem == true) {return;}
                else
                PickUpItem(other.gameObject);               
            }
        }

// Server adds reward to a player and destroy pickup
        [ServerCallback]
        public void PickUpItem(GameObject player)
        {
            if (type == "Reward")
            player.GetComponent<PlayerScore>().hasItem = true;
            NetworkServer.Destroy(gameObject);

            if (type =="Stealing")
            player.GetComponent<PlayerScore>().canSteal = true;
            NetworkServer.Destroy(gameObject);
        }


    }
}