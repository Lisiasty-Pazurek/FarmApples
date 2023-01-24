using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mirror.Discovery
{
    [HelpURL("https://mirror-networking.gitbook.io/docs/components/network-discovery")]
    [RequireComponent(typeof(NetworkDiscovery))]
    public class DiscoveryUI : MonoBehaviour
    {
        readonly Dictionary<long, ServerResponse> discoveredServers = new Dictionary<long, ServerResponse>();
        Vector2 scrollViewPos = Vector2.zero;

        public NetworkDiscovery networkDiscovery;

        void OnValidate()
        {
            // if (networkDiscovery == null)
            // {
            //     networkDiscovery = GetComponent<NetworkDiscovery>();
            //     UnityEditor.Events.UnityEventTools.AddPersistentListener(networkDiscovery.OnServerFound, OnDiscoveredServer);
            //     UnityEditor.Undo.RecordObjects(new Object[] { this, networkDiscovery }, "Set NetworkDiscovery");
            // }
            // FindServers();
        }

        public void FindServers()
        {
            discoveredServers.Clear();
            networkDiscovery.StartDiscovery();
        }
        public void StartLocalHost()
        {
            discoveredServers.Clear();
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();
        }

        public void DisplayServers() {
            // foreach (ServerResponse info in discoveredServers.Values)
            // if (GUILayout.Button(info.EndPoint.Address.ToString()))
            // Connect(info);

            // GUILayout.EndScrollView();
            // GUILayout.EndArea();
        }

        void Connect(ServerResponse info)
        {
            networkDiscovery.StopDiscovery();
            NetworkManager.singleton.StartClient(info.uri);
        }

        public void OnDiscoveredServer(ServerResponse info)
        {
            // Note that you can check the versioning to decide if you can connect to the server or not using this method
            discoveredServers[info.serverId] = info;
        }

    }

}
