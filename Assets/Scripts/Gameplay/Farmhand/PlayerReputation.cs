
using UnityEngine;
using Mirror;

public class PlayerReputation : NetworkBehaviour
{
    public SyncDictionary<string, int> reputation = new SyncDictionary<string, int>();
}
