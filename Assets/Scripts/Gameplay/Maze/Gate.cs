using UnityEngine;
using Mirror;
using MirrorBasics;

public class Gate : NetworkBehaviour 
{
    public MeshCollider gateCollider;
    public GameObject gateRenderer;

    [Server]
    void OpenGate()
    {
        gateCollider.enabled = false;
        gateRenderer.SetActive(false);
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<PlayerScore>() != null ) 
        {   
            if (other.GetComponent<PlayerScore>().hasItem)
            OpenGate();
        }
        
    }

}
