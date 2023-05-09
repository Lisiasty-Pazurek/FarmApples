using UnityEngine;
using Mirror;
using System.Collections.Generic;

namespace MirrorBasics{
public class Spawner: NetworkBehaviour
{
    [SerializeField]  GameObject rewardPrefab;
    [SerializeField]  GameObject pickupPrefab;
    [SerializeField]  GameObject stealingPrefab;

    [SerializeField] public GameObject teampointPrefab;
    [SerializeField] public GameObject teampointPrefab2;

    private GameObject teamboxPrefab;

    [SerializeField] public GameObject cauldronPrefab1;
    [SerializeField] public GameObject cauldronPrefab2;


    [SerializeField] public List<Transform> playerSpawnPoints = new List<Transform>();
    [SerializeField] public List<Transform> rewardSpawnPoints = new List<Transform>();
    [SerializeField] public List<Transform> teamSpawnPoints = new List<Transform>();
    [SerializeField] public List<Transform> pickupSpawnPoints = new List<Transform>();  

    public GameMode gameMode;

    private LevelController lvlController;

    public Transform startLocation;  
    public Vector3 spawnPosition; 

    public override void OnStartServer() 
    {
        lvlController = gameObject.GetComponentInParent<LevelController>();
        gameMode = gameObject.GetComponentInParent<GameMode>();
        SetLocation(startLocation);
        InitialSpawn();
    }

    [Server]
    public void SetLocation(Transform setLocation)
    {
        startLocation = setLocation;
    }
    [Server]
    internal void InitialSpawn()
    {
        if (!NetworkServer.active) return;
 
        for (int i = 0; i < gameMode.maxRewards; i++)
        SpawnPickup(rewardPrefab);

        for (int i = 0; i < gameMode.maxBonuses; i++)
        SpawnPickup(stealingPrefab);

    }
    [Server]
    internal void SpawnPickup(GameObject spawnPrefab)
    {
        if (!NetworkServer.active) return;

        if (rewardSpawnPoints.Count == 0)
        {
            spawnPosition = new Vector3(Random.Range(startLocation.position.x-180,startLocation.position.x +180), 0, Random.Range(startLocation.position.z-10,startLocation.position.z +120));                
        }
        if (rewardSpawnPoints.Count > 0)       
        {
            int x = Random.Range(0, rewardSpawnPoints.Count);
            print(x);
            spawnPosition = rewardSpawnPoints[x].position;
            rewardSpawnPoints.Remove(rewardSpawnPoints[x]);
        }
        print(spawnPosition);
        GameObject pickup = Instantiate(spawnPrefab, spawnPosition, Quaternion.identity );
        // pickup.GetComponent<NetworkMatch>().matchId = this.GetComponent<NetworkMatch>().matchId;            
        NetworkServer.Spawn(pickup);
        lvlController.spawnedItems.Add(pickup);
        if (pickup.GetComponentInChildren<MeshRenderer>() != null){pickup.GetComponentInChildren<MeshRenderer>().enabled = true; }                
    }

    [Server]
    private void SpawnRecipeIngredients(GameObject spawnPrefab) 
    {
        if (!NetworkServer.active) return;

        List<string> ingredientsToSpawn = new List<string>();
        foreach (Cauldron recipe in FindObjectsOfType<Cauldron>())
        {
            ingredientsToSpawn.AddRange(recipe.ingredientList);
        }


        for (int i = 0; i < ingredientsToSpawn.Count; i++)
        {
            int x = Random.Range(0, rewardSpawnPoints.Count);
            print(x);
            spawnPosition = rewardSpawnPoints[x].position;
            rewardSpawnPoints.Remove(rewardSpawnPoints[x]);

            GameObject pickup = Instantiate(spawnPrefab, spawnPosition, Quaternion.identity );         
            pickup.GetComponent<Pickup>().pickupName = ingredientsToSpawn[i];
            pickup.GetComponent<Ingredient>().ingredientName = ingredientsToSpawn[i];
            NetworkServer.Spawn(pickup);            
            //if (pickup.GetComponentInChildren<MeshRenderer>() != null){pickup.GetComponentInChildren<MeshRenderer>().enabled = true; }                 
        }



    }

public void SpawnTeamboxes()
{
    for (int i = 0; i < gameMode.maxTeams;)
    { 
        if (!IsOdd(i))
        {
            Debug.Log("sending spawnpoint: " + i);
            SpawnTeambox(1, i);
        }
        else 
        {
            SpawnTeambox(2, i);
        }
        i++;
    }

    if (gameMode.gameModeName =="Farmcook") SpawnRecipeIngredients(pickupPrefab);
}

[Server]
    public void SpawnTeambox(int prefab, int spawnPosition)
    {
        if (gameMode.gameModeName == "Farmapples" ) 
        {
            if ( prefab == 1) { teamboxPrefab = teampointPrefab;}
            if ( prefab == 2) { teamboxPrefab = teampointPrefab2;}

            Debug.Log("Spawner got gameobject prefab of name: " + teamboxPrefab.name + "and spawnPosition index of: " + spawnPosition);
            GameObject teambox = Instantiate(teamboxPrefab);
            NetworkServer.Spawn(teambox);
            teambox.transform.position = teamSpawnPoints[spawnPosition].position;
            teambox.GetComponent<TeamBox>().requiredScore = gameMode.maxScore;                
        }

        if (gameMode.gameModeName == "Farmcook")
        {
            if(prefab == 1) { teamboxPrefab = cauldronPrefab1;}
            if(prefab == 2) { teamboxPrefab = cauldronPrefab2;}       
            Debug.Log("Spawner got gameobject prefab of name: " + teamboxPrefab.name + "and spawnPosition index of: " + spawnPosition);
            GameObject teambox = Instantiate(teamboxPrefab);
            NetworkServer.Spawn(teambox);
            teambox.transform.position = teamSpawnPoints[spawnPosition].position;         
        }

 //           lvlController.spawnedItems.Add(teambox);           
    }

        /// #### supporting function
    public static bool IsOdd(int value)
    {
        return value % 2 != 0;
    }

    }
}