using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class BossAnimController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public Action animEnded;
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

    public void AnimEnded()
    {
        animator.enabled = false;

        animEnded?.Invoke();
    }



}
