using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MirrorBasics {
public class GameMode : MonoBehaviour
{
    public string gameModeName;
    public string mapName;
    public int minTeams;
    public int maxTeams;
    public int maxScore;

    public int maxRewards;
    public int maxBonuses;

    public Scene gameScene;
    public List<GameObject> levelPrefab = new List<GameObject>();
    
    public void  Start() 
    {

    }
    public void SetGameMode()
    {
        
    }


}
}