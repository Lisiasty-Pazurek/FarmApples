using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorBasics 
{
    public class UIScore : MonoBehaviour
    {
        private UIGameplay uiGameplay;
        [SerializeField] private Text playerName;
        [SerializeField] private Text playerScore;        
        [SerializeField] private Text team1Score;
        [SerializeField] private Text team2Score;
        void Start() 
        {

        }

        public void SetPlayerName (Player player) 
        {
//            player = FindObjectOfType<Player>(); // at game level start = if (isClient) {UIScore.instance.SetPlayername(localPlayer);}
            playerName.text = player.name;
        }

        public void SetPlayerScore(PlayerScore player)
        {
            // at score change if (isClient) UIScore.instance.SetPlayeScore();
            playerScore.text = player.score.ToString(); 
        }

        private void SetTeamScore()
        {
     //       team1Score
        }

        private void SetStatus()
        {

        }

    }
}