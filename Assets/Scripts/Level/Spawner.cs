using UnityEngine;
using Mirror;
using System.Collections.Generic;

namespace MirrorBasics{
public class Spawner: NetworkBehaviour
    {
        [SerializeField]  GameObject rewardPrefab;
        [SerializeField]  GameObject stealingPrefab;

        [SerializeField] public GameObject teampointPrefab;
        [SerializeField] public GameObject teampointPrefab2;
        private GameObject teamboxPrefab;

    [SerializeField] public List<Transform> playerSpawnPoints;
    [SerializeField] public List<Transform> rewardSpawnPoints;
    [SerializeField] public List<Transform> teamSpawnPoints;
    [SerializeField] public List<Transform> pickupSpawnPoints;  

        public GameMode gameMode;

        private LevelController lvlController;

        public Transform startLocation;  

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
            Vector3 spawnPosition = new Vector3(Random.Range(startLocation.position.x-180,startLocation.position.x +180), 0, Random.Range(startLocation.position.z-10,startLocation.position.z +120));
            GameObject pickup = Instantiate(spawnPrefab, spawnPosition, Quaternion.identity);
            // pickup.GetComponent<NetworkMatch>().matchId = this.GetComponent<NetworkMatch>().matchId;            
            NetworkServer.Spawn(pickup);
            lvlController.spawnedItems.Add(pickup);
            pickup.GetComponentInChildren<MeshRenderer>().enabled = true;       
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
    }

    [Server]
        public void SpawnTeambox(int prefab, int spawnPosition)
        {
            if (prefab == 1) { teamboxPrefab = teampointPrefab;}
            if (prefab == 2) { teamboxPrefab = teampointPrefab2;}
            Debug.Log("Spawner got gameobject prefab of name: " + teamboxPrefab.name + "and spawnPosition index of: " + spawnPosition);
            GameObject teambox = Instantiate(teamboxPrefab);
            NetworkServer.Spawn(teambox);
            teambox.transform.position = teamSpawnPoints[spawnPosition].position;
            teambox.GetComponent<TeamBox>().requiredScore = gameMode.maxScore;
 //           lvlController.spawnedItems.Add(teambox);
            
        }

        /// #### supporting function
    public static bool IsOdd(int value)
    {
        return value % 2 != 0;
    }

    }
}