using Mirror;
using UnityEngine;

namespace MirrorBasics 
{
public class TeamBox : NetworkBehaviour
{   
    [SyncVar]
    public int teamID ;
    [SyncVar]
    public int teamPoints;
    [SerializeField]
    private int requiredScore = 10;
    [SerializeField]


    private LevelController levelController;


    public override void OnStartServer() 
    {
        Debug.Log("Teambox validated, setting up levelcontroller in it");
        levelController = GameObject.FindObjectOfType<LevelController>();
    }


// Checking if player approaching teambox is a same team and if he has item, if true = run ClaimPrize to add personal and team points. 
[Server]
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerScore>().teamID == teamID)
            {
                if (other.gameObject.GetComponent<PlayerScore>().hasItem == false) {return;}
                ClaimPrize(other.gameObject);               
            }
        }


[Server]
        public void ClaimPrize(GameObject player)
        {
            HandleScoreChange(player);

                // increase teamPoints score on teamBox object
                teamPoints +=1;
                Debug.Log("Team " + teamID + " points: " + teamPoints);

                // check level requirements and start ending game
                if (teamPoints > requiredScore)
                {
                    Debug.Log("Ending game");
                    levelController.gameEnded = true;
                    ServerEndGame();
                }
        }


[Server]
    public void ServerEndGame() 
    {
        levelController.EndLevel();
    }

[ClientRpc]
    public void ClientSceneAfterEndGame() 
    {
        // NetworkClient.Ready();
        // networkManager.showRoomGUI = true;
    }

[Client]
    public void HandleScoreChange(GameObject player)
    {
                // award the points via SyncVar on the PlayerController
                player.GetComponent<PlayerScore>().score += 1;

                // destroy this object
                player.gameObject.GetComponent<PlayerScore>().hasItem = false;
    }
}

}
