using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public MeshCollider gateCollider;
    public GameObject gateRenderer;
    void OpenGate()
    {
        gateCollider.enabled = false;
        gateRenderer.SetActive(false);
    }

}
