using Mirror;
using UnityEngine;
using MirrorBasics;


public class PlayerAnimator : NetworkBehaviour 
{

    [SerializeField]
    private PlayerController playerController;
    private NetworkAnimator networkAnimator;
    [SerializeField]
    private Animator characterAnimator;
    [SerializeField]
    private Animator modelAnimator;

    void OnValidate()  {   }

    public override void OnStartLocalPlayer()
    {
        if (characterAnimator == null)
        {
            characterAnimator = GetComponent<Animator>();
            characterAnimator.enabled = false;
        }
        if (networkAnimator == null)
        {
            networkAnimator = GetComponent<NetworkAnimator>();
            networkAnimator.enabled = false;
        }

        // modelAnimator = GetComponentsInChildren<Animator>()[1];
        // characterAnimator.avatar = modelAnimator.avatar;
        // characterAnimator.runtimeAnimatorController = modelAnimator.runtimeAnimatorController ;
        characterAnimator.enabled = true;
        networkAnimator.enabled = true;
    }

    private void FixedUpdate() 
    {
        if (!isLocalPlayer || characterAnimator == null || !characterAnimator.enabled)
        return;
        
        
            characterAnimator.SetBool("Walking", playerController.velocity != Vector3.zero);
            characterAnimator.SetBool("Idle", playerController.velocity == Vector3.zero);
            characterAnimator.SetBool("Rolling", playerController.isDashing);

    }



}
