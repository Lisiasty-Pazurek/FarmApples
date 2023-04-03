using System.Collections.Generic;
using UnityEngine;
using MirrorBasics;

public class Runner : MonoBehaviour
{
    [SerializeField]public Dictionary <int, float>  visitedCheckpoints = new Dictionary<int, float>();

    PlayerController pController;
    LevelController levelManager;
    public GameObject timeScorePrefab;
    public Transform timeScoreLocation;

    void Start ()
    {
        pController = GetComponent<PlayerController>();
        visitedCheckpoints.Add(0, 0);
        timeScoreLocation = FindObjectOfType<UIScore>().scoreRowLocation;
    }

    public void VisitCheckpoint(int id, float time)
    {
        if (visitedCheckpoints.ContainsKey(id)){return;}
        else
        visitedCheckpoints.Add(id, time);
        GameObject ScorePrefab = Instantiate(timeScorePrefab, timeScoreLocation);
        ScorePrefab.GetComponent<TimeScore>().id.text = id.ToString();
        ScorePrefab.GetComponent<TimeScore>().time.text = time.ToString();       
        Debug.Log("Checkpoint reached at: " + visitedCheckpoints);
    }


}
