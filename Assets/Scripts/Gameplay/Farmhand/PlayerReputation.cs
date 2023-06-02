
using UnityEngine;
using Mirror;

public class PlayerReputation : NetworkBehaviour
{
    public SyncList<string> reputation = new SyncList<string>();

    public void Start()
    {
        AddReputation("zadanie");
    }

    [Command]
    public void AddReputation (string item)
    {
        if (!reputation.Contains(item))
        {
            reputation.Add(item);
        }
    }
}
