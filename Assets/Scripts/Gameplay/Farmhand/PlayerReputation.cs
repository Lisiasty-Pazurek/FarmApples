
using UnityEngine;
using Mirror;

public class PlayerReputation : NetworkBehaviour
{
    public SyncList<string> reputation = new SyncList<string>();
}
