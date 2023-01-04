using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class Spawner: NetworkBehaviour
    {
        [SerializeField]
        GameObject rewardPrefab;

        public Transform startLocation;  

        public void SetLocation(Transform setLocation)
        {
            startLocation = setLocation;
        }
        internal void InitialSpawn()
        {
            if (!NetworkServer.active) return;

            for (int i = 0; i < 10; i++)
                SpawnReward();
        }

        public override void OnStartServer() 
        {
            SetLocation(startLocation);
            InitialSpawn();
        }

        internal void SpawnReward()
        {
            if (!NetworkServer.active) return;

            Vector3 spawnPosition = new Vector3(Random.Range(startLocation.position.x-30,startLocation.position.x +30), 1, Random.Range(startLocation.position.z-30,startLocation.position.z +30));
            NetworkServer.Spawn(Object.Instantiate(rewardPrefab, spawnPosition, Quaternion.identity));
            SceneManager.MoveGameObjectToScene(rewardPrefab, SceneManager.GetActiveScene());
        }
    }
