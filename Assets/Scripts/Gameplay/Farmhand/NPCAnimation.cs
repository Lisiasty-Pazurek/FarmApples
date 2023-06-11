using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NPCAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] private List<AnimationLoop> animations; 
    private int currentIndex;

    void Start()
    {
        currentIndex = 0;
        animator.speed = .6f;
        StartCoroutine(SetAnimation());
    }

    public IEnumerator SetAnimation()
    {
        while (true)
        {
            print("animating 1");
            if (animator.GetBool(animations[currentIndex].animationState) != true)
            { animator.SetBool(animations[currentIndex].animationState, true);}

            yield return new WaitForSeconds(animations[currentIndex].animationTime);
            if (animator.GetBool(animations[currentIndex].animationState) == true)
            {animator.SetBool(animations[currentIndex].animationState, false);}
            currentIndex = (currentIndex + 1) % animations.Count();            
        }
        
    }

    [System.Serializable]
    public struct AnimationLoop 
    {
        public string animationState;
        public float animationTime;
    }

}
