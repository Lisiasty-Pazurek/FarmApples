using UnityEngine;
using Mirror;
using MirrorBasics;

public class EndPoint : NetworkBehaviour
{

    [SerializeField] private LevelController levelController;

    public override void OnStartServer() 
    {
        levelController = GameObject.FindObjectOfType<LevelController>();
    }

    public override void OnStartLocalPlayer()    {    }

    // Checking if player approaching teambox is a same team and if he has item, if true = run ClaimPrize to add personal and team points. 
    [ServerCallback]
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                if (other.gameObject.GetComponent<PlayerScore>().hasItem == false) {return;}
                ServerEndGame();               
            }
        }

    [Server]
    public void ServerEndGame() 
    {
        levelController.EndLevel(0);
    }
}
