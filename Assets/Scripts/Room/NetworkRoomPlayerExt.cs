using UnityEngine;
using Mirror;

namespace MirrorBasics
{
//    [AddComponentMenu("")]
    public class NetworkRoomPlayerExt : NetworkRoomPlayer
    {
        public static NetworkRoomPlayerExt localPlayer;
        public UIRoom uiRoom;
        
        public override void OnStartClient()
        {
            //Debug.Log($"OnStartClient {gameObject}");
   //         if (isLocalPlayer) { localPlayer = this;}
            uiRoom = FindObjectOfType<UIRoom>();
        }

        public override void OnClientEnterRoom()
        {
            //Debug.Log($"OnClientEnterRoom {SceneManager.GetActiveScene().path}");
            if (isLocalPlayer)
            {
                uiRoom.roomPlayer = this;
            }
        }

        public override void OnClientExitRoom()
        {
            //Debug.Log($"OnClientExitRoom {SceneManager.GetActiveScene().path}");
        }

        public override void IndexChanged(int oldIndex, int newIndex)
        {
            //Debug.Log($"IndexChanged {newIndex}");
        }

        public override void ReadyStateChanged(bool oldReadyState, bool newReadyState)
        {
            //Debug.Log($"ReadyStateChanged {newReadyState}");
        }

        public override void OnGUI()
        {
            base.OnGUI();
        }
    }
}
