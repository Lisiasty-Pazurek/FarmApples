using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionStatus : MonoBehaviour
{
    [SerializeField] private LightReflectiveMirror.LightReflectiveMirrorTransport LRMTransport;
    [SerializeField] private Animator animator;

    void Start()
    {
        CheckStatus();
        StartCoroutine(RelayStatus());
    }

    IEnumerator RelayStatus ()
    {
        while(Application.isPlaying)
        {
            CheckStatus();
            yield return new WaitForSeconds(3);
        }
        yield return null;
    }

    public void CheckStatus()
    {
        if (animator != null)
        {
            animator.SetBool("connected", LRMTransport.Available());  
        }   
    }


}
