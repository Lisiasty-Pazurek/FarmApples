using Mirror;
using UnityEngine;
using UnityEngine.UI;

namespace MirrorBasics {
    public class AutoHostClient : MonoBehaviour {

        [SerializeField] NetworkManager networkManager;
        [SerializeField] InputField serverAddress;

        void Start () {
            if (!Application.isBatchMode) { //Headless build
                Debug.Log ($"=== Client Build ===");
                networkManager.StartClient ();
            } else {
                Debug.Log ($"=== Server Build ===");
                networkManager.StartServer();
            }

        }

        public void JoinLocal () {
            networkManager.networkAddress = "localhost";
            networkManager.StartClient ();
        }

        public void JoinServer () {
            networkManager.networkAddress = "89.78.252.220";
            networkManager.StartClient ();
        }

        public void StartServer()
        {
            networkManager.StartServer();
        }

        public void SetServerAddress()
        {
            networkManager.networkAddress = serverAddress.text.ToString();
        }

    }
}