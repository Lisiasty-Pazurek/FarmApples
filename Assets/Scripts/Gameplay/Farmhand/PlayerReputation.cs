
using UnityEngine;
using Mirror;

public class PlayerReputation : NetworkBehaviour
{
    [SerializeField] private bool reportReputationChange;
    [SerializeField] private PlayerController pController;
    public readonly SyncList<string> reputation = new SyncList<string>();

    public void Start()
    {
        pController = GetComponent<PlayerController>();
    }

    [Command]
    public void AddReputation (string item)
    {
        if (!reputation.Contains(item))
        {
            reputation.Add(item);
            if (isLocalPlayer) {pController.uiGameplay.GetComponent<UIReputationSystem>().LoadIcon(item);}            
            if (reportReputationChange && item != "")
            {
                SendReport.instance.SetAndSendMessage(GetComponent<PlayerController>().playerName, item);
            }
        }
    }

    [Command]
    public void RemoveReputation (string item)
    {
        if (reputation.Contains(item))
        {
            reputation.Remove(item);
            if (isLocalPlayer) {pController.uiGameplay.GetComponent<UIReputationSystem>().UnloadIcon(item);}    
        }
    }

}
