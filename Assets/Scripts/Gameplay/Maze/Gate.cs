using UnityEngine;
using Mirror;
using MirrorBasics;



public class Gate : NetworkBehaviour 
{
    public MeshCollider gateCollider;
    public GameObject gateRenderer;
    void OpenGate()
    {
        gateCollider.enabled = false;
        gateRenderer.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.GetComponent<PlayerScore>().hasItem) 
        {
            OpenGate();
        }
        
    }

}
