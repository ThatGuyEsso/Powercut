using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHooverAnimations : MonoBehaviour
{
    private Animator animator;
    RectTransform rt;
    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        rt = gameObject.GetComponent<RectTransform>();
        animator.enabled = false;
    }


    public void OnHoover()
    {
        animator.enabled = true;
        animator.Play("HighlightedAnimation");
    }
    public void OnFinishedHoover()
    {
  
        animator.enabled = false;
        
    }

    public void OnLeave()
    {
        animator.enabled = true;
        animator.Play("UnHiglightedAnim");
    }
    public void OnFinishedLeave()
    {
        animator.enabled = false;
    }
}
