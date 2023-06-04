
using UnityEngine;
using Mirror;

public class PlayerReputation : NetworkBehaviour
{
    public readonly SyncList<string> reputation = new SyncList<string>();
    [SerializeField] private bool reportReputationChange;

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
            if (reportReputationChange)
            {
                SendReport.instance.SetAndSendMessage(GetComponent<PlayerController>().playerName, item);
            }
        }
    }
}
