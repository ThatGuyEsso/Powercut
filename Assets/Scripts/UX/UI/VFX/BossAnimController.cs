using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAnimController : MonoBehaviour
{
    [SerializeField] private Animator animator;


    public void Awake()
    {
        if (animator)
            animator.enabled = false; 
    }
    public void PlayAnim(string animName)
    {
        animator.enabled = true;
        animator.Play(animName);
    }




}
