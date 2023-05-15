using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

namespace MirrorBasics 
{
    public class UIScore : MonoBehaviour
    {
        public PlayerScore player;     
        [SerializeField] private Text playerScore;        
        [SerializeField] private Text team1Score;
        [SerializeField] private Text team2Score;
        public Transform scoreRowLocation;

        [SerializeField] private List<GameObject> statusImage = new List<GameObject>();

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
            if (status == "rolling") statusImage[0].SetActive(state);
            if (status == "apple") statusImage[1].SetActive(state);
            if (status == "stealing") statusImage[2].SetActive(state);
            else return;
        }

        public void ResetValues()
        {
            SetPlayerScore(0);
            SetTeamScore(1,0);
            SetTeamScore(2,0);
        }

    }
}