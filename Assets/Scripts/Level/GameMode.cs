using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMode : MonoBehaviour
{
    public string GameModeName;
    public int minPlayers;
    public int maxPlayers;
    public int minTeams;
    public int maxTeams;
    public int maxScore;

    [SerializeField] public List<Transform> playerSpawnPoints;
    [SerializeField] public List<Transform> rewardSpawnPoints;
    [SerializeField] public List<Transform> teamSpawnPoints;
    [SerializeField] public List<Transform> pickupSpawnPoints;  
    public Scene gameScene;
    
    

}
