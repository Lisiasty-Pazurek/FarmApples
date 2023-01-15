using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

namespace MirrorBasics{
public class Spawner: NetworkBehaviour
    {
        [SerializeField]
        GameObject rewardPrefab;
        private LevelController lvlController;

        public Transform startLocation;  
        public override void OnStartServer() 
        {
            lvlController = gameObject.GetComponentInParent<LevelController>();
            SetLocation(startLocation);
            InitialSpawn();
        }

        public void SetLocation(Transform setLocation)
        {
            startLocation = setLocation;
        }
        internal void InitialSpawn()
        {
            if (!NetworkServer.active) return;

            for (int i = 0; i < 20; i++)
            
            SpawnReward();
        }



        internal void SpawnReward()
        {
            if (!NetworkServer.active) return;

            Vector3 spawnPosition = new Vector3(Random.Range(startLocation.position.x-60,startLocation.position.x +60), 1, Random.Range(startLocation.position.z-30,startLocation.position.z +30));

            GameObject prize = Instantiate(rewardPrefab, spawnPosition, Quaternion.identity);
            prize.GetComponent<NetworkMatch>().matchId = this.GetComponent<NetworkMatch>().matchId;            
            NetworkServer.Spawn(prize);
            lvlController.spawnedItems.Add(prize);
            prize.GetComponentInChildren<MeshRenderer>().enabled = true;       
        }
    }
}