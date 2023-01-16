using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorBasics 
{
    public class UIScore : MonoBehaviour
    {
        public static UIScore uiScore;
        public PlayerScore player;
        public Player lobbyPlayer;
//        public TeamBox teambox;
//        private UIGameplay uiGameplay;
        [SerializeField] private Text playerName;
        [SerializeField] private Text playerScore;        
        [SerializeField] private Text team1Score;
        [SerializeField] private Text team2Score;

        [SerializeField] private List<GameObject> statusImage = new List<GameObject>();

        void Start() 
        {   }

        public void SetPlayerName () 
        {
           lobbyPlayer = Player.localPlayer; // GameObject.FindObjectOfType<Player>(); // at game level start = if (isClient) {UIScore.instance.SetPlayername(localPlayer);}
           playerName.text = lobbyPlayer.name;
//            Debug.Log("Setting planer name for " + player.playerIndex);
        }

        public void SetPlayerScore(int score)
        {
            // at score change if (isClient) UIScore.instance.SetPlayeScore();
            playerScore.text = score.ToString(); 
        }

        public void SetTeamScore(int teamID, int teamPoints)
        {
            if (teamID ==1){team1Score.text = teamPoints.ToString(); }
            if (teamID ==2){team2Score.text = teamPoints.ToString(); }
            else return;
        }

        public void SetStatusIcon (string status, bool state)
        {
            
        }



    }
}