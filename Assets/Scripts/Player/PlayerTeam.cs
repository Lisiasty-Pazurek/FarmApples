
using UnityEngine;
using Mirror;

namespace MirrorBasics {
    public class PlayerTeam : NetworkBehaviour
    {
        [SyncVar] public int teamID;
    }
}