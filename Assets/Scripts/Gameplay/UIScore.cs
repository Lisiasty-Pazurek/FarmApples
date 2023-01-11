using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorBasics 
{
    public class UIScore : MonoBehaviour
    {
        public static UIScore instance;

        [SerializeField] private Text playerName;
        [SerializeField] private Text playerScore;        
        [SerializeField] private Text team1Score;
        [SerializeField] private Text team2Score;
        void Start() 
        {

        }

        public void SetPlayerName (Player player) 
        {
//            player = FindObjectOfType<Player>(); // at game level start = if (isClient) UIScore.instance.SetPlayername(localPlayer);
            playerName.text = player.name;
        }

        private void SetPlayerScore()
        {
            // at score change if (isClient) UIScore.instance.SetPlayeScore();
            // playerScore.text = localPlayer.GetComponent<PlayerScore>().score.ToString(); or
        }

        private void SetTeamScore()
        {

        }

        private void SetStatus()
        {

        }

    }
}