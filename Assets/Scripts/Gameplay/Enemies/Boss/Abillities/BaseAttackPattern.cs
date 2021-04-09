using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public abstract class BaseAttackPattern : MonoBehaviour
{
    //Pattern Data
    protected BossStage stage;
    protected float attackRate;
    protected float attackDuration;
    protected float attackCoolDown;
    protected int attackCount;
    protected int maxAttackCount;


    //pattern Settings
    [SerializeField] protected float attackRange;
    [SerializeField] protected Transform playerTransform;

    public Action AttackEnded;


    //pattern state
    protected bool isRunning;
    public void SetUpAbilityData(AttackPatternData patternData)
    {
        stage = patternData.Stage;
        attackRate = patternData.AttackRate;
        attackDuration = patternData.AttackDuration;
        attackCoolDown = patternData.Cooldown;
        attackCount = patternData.AttackCount;
        maxAttackCount = patternData.MaxAttackCount;
    }

    public void SetStage(BossStage newStage) { stage = newStage; }

    protected void BeginAttackPattern()
    {
        isRunning = true;
        StartCoroutine(BeginAttackCycle());
        if (attackDuration > 0f)
            Invoke("StopRunning", attackDuration);


    }

    virtual protected void StopRunning()
    {
        StopAllCoroutines();
        isRunning = false;
        AttackEnded.Invoke();
    }

    protected IEnumerator BeginAttackCycle()
    {
        yield return new WaitForSeconds(attackRate);
        ExecuteAttack();
        if (isRunning)
        {
            StartCoroutine(BeginAttackCycle());

        }
    }
    protected abstract void ExecuteAttack();
 


}
