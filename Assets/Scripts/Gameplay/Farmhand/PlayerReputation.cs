
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

    [ClientCallback]
    public void AddReputation (string item)
    {
        if (isLocalPlayer) {pController.uiGameplay.GetComponent<UIReputationSystem>().LoadIcon(item);}      
        CmdAddReputation(item);
    }

    [Command]
    public void CmdAddReputation(string item)
    {
        if (!reputation.Contains(item))
        {
            reputation.Add(item);   
        }     
        if (reportReputationChange && item != "")
        {
            SendReport.instance.SetAndSendMessage(GetComponent<PlayerController>().playerName, item);
        }        
    }



    [ClientCallback]
    public void RemoveReputation (string item)
    {
        if (reputation.Contains(item))
        {
            CmdRemoveReputation(item);
            if (isLocalPlayer) {pController.uiGameplay.GetComponent<UIReputationSystem>().UnloadIcon(item);}    
        }
    }

    [Command]
    public void CmdRemoveReputation(string item)
    {
        if (reputation.Contains(item))
        {
            reputation.Remove(item);   
        }    
    }

}
