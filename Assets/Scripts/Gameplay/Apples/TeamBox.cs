using Mirror;
using UnityEngine;

namespace MirrorBasics 
{
public class TeamBox : NetworkBehaviour
{   
    [SyncVar] public int teamID ;
    [SyncVar] public int teamPoints;
    [SerializeField][SyncVar] public int requiredScore = 5;
    [SerializeField] private LevelController levelController;
    private UIScore uiScore;

    public override void OnStartServer() 
    {
        Debug.Log("Teambox validated, setting up levelcontroller in it");
        levelController = GameObject.FindObjectOfType<LevelController>();
    }

    public override void OnStartLocalPlayer()
    {
        uiScore = GameObject.FindObjectOfType<UIScore>();        
    }


// Checking if player approaching teambox is a same team and if he has item, if true = run ClaimPrize to add personal and team points. 
    [ServerCallback]
        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerScore>().teamID == teamID )
            {
                if (other.gameObject.GetComponent<PlayerScore>().hasItem == false) {return;}
                ClaimPrize(other.gameObject);               
            }
        }


    [Server]
        public void ClaimPrize(GameObject player)
        {
        
                // increase teamPoints score on teamBox object
                teamPoints++;            
                player.GetComponent<PlayerScore>().score ++;
                player.GetComponent<PlayerScore>().hasItem = false; 
                Debug.Log("Team " + teamID + " points: " + teamPoints);
                RpcTeamScoreUpdate();

                // check level requirements and start ending game
                if (teamPoints >= requiredScore)
                {
                    Debug.Log("Ending game");
                    levelController.gameEnded = true;
                    ServerEndGame();
                }
        }

        // public void HandleScoreChange(GameObject player)
        // {
        //     // award the points via SyncVar on the PlayerController

        // }
    
    [ClientRpc]
    public void RpcTeamScoreUpdate()
    {
        ClientUpdateTeamScore();
    }

    [Client]
    public void ClientUpdateTeamScore ()
    {
        uiScore = GameObject.FindObjectOfType<UIScore>();
        uiScore.SetTeamScore(teamID, teamPoints);
    }

    [Server]
    public void ServerEndGame() 
    {
        levelController.EndLevel(teamID);
    }

}

}
