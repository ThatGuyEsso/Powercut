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
    protected AudioSource aSource;
    [SerializeField] protected string sfxName;

    //pattern Settings
    [SerializeField] protected float attackRange;
    [SerializeField] protected float damage;
    [SerializeField] protected float knockBack;

    [HideInInspector] public Transform playerTransform;

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

    virtual public void BeginAttackPattern()
    {
        isRunning = true;
        if(gameObject.activeSelf)
            StartCoroutine(BeginAttackCycle());
        if (attackDuration > 0f)
            Invoke("StopRunning", attackDuration);


    }
    private void OnDisable()
    {
        DisableAttack();
    }

    virtual public void StopRunning()
    {
        StopAllCoroutines();
        isRunning = false;
        AttackEnded?.Invoke();
    }

    virtual public void DisableAttack()
    {
        StopAllCoroutines();
        isRunning = false;
   
    }

    virtual protected IEnumerator BeginAttackCycle()
    {
        yield return new WaitForSeconds(attackRate);
        ExecuteAttack();
        if (isRunning)
        {
            StartCoroutine(BeginAttackCycle());

        }
    }
    public abstract void ExecuteAttack();


    //public abstract void ResetAttack();
}
