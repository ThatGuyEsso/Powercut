using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveShield : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject hiveGFX;
    [SerializeField] private Collider2D shieldCollider;
    private BroodNest hive;
    private void Awake()
    {
        if (animator)
            animator.enabled = false;
        if (hiveGFX)
            hiveGFX.SetActive(false);

        hive = GetComponentInParent<BroodNest>();
        shieldCollider.enabled = false;
    }

    public void ResetShield()
    {
        if (animator)
            animator.enabled = false;
        if (hiveGFX)
            hiveGFX.SetActive(false);
        shieldCollider.enabled = false;
    }
    public void ShieldBuilt()
    {
        animator.enabled = false;
        hive.SpawnBroodDelegates();
    }
    public void ShieldRemoved()
    {
        animator.enabled = false;
        hiveGFX.SetActive(false);
        shieldCollider.enabled = false ;
        hive.EvaluateBossStage();
    }


    public void PlayAnimation(string animName)
    {
        shieldCollider.enabled = true;
        animator.enabled = true;
        hiveGFX.SetActive(true);
        animator.Play(animName);
    }
}
