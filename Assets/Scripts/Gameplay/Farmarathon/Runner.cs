using System.Collections.Generic;
using UnityEngine;
using Mirror;
using MirrorBasics;

public class Runner : NetworkBehaviour
{
    [SerializeField]public readonly SyncDictionary <int, float>  visitedCheckpoints = new SyncDictionary<int, float>();

    PlayerController pController;
    LevelController levelManager;
    public GameObject timeScorePrefab;
    public Transform timeScoreLocation;

    public override void OnStartServer ()
    {
        pController = GetComponent<PlayerController>();
        visitedCheckpoints.Add(0, 0);
        timeScoreLocation = FindObjectOfType<UIScore>().scoreRowLocation;
    }

    public override void OnStartLocalPlayer ()
    {
        timeScoreLocation = FindObjectOfType<UIScore>().scoreRowLocation;
    }

    [Server]
    public void VisitCheckpoint(int id, float time)
    {
        if (visitedCheckpoints.ContainsKey(id)){return;}
        else
        visitedCheckpoints.Add(id, time);
        SpawnUIScore(id, (int)time);
        Debug.Log("Checkpoint reached at: " + visitedCheckpoints);
    }

    [TargetRpc]
    private void SpawnUIScore(int id, int time)
    {
        Debug.Log("Client pre local player");

        GameObject ScorePrefab = Instantiate(timeScorePrefab, timeScoreLocation);
        ScorePrefab.GetComponent<TimeScore>().id.text = id.ToString();
        ScorePrefab.GetComponent<TimeScore>().time.text = time.ToString();             
        
    }


}
