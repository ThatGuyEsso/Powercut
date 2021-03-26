using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroodSpitter : BaseEnemy
{

    [SerializeField] protected Transform firePoint;
    [SerializeField] protected float ShotCooldown;
   
    [SerializeField] protected float shotForce;
    protected bool canAttack = true;
    protected override void Awake()
    {
        base.Awake();

 
        navComp.Init();

        animController = gameObject.GetComponent<BaseEnemyAnimController>();
        animController.Init();

        //Initiate mutation of base character
  

    }
    private void Start()
    {
        if (target != false)
        {
            SetEnemyState(EnemyStates.Chase);
            navComp.enabled = true;
        }
        else
        {
            navComp.enabled = false;
        }

    }

    protected override void Update()
    {
        base.Update();

        switch (currentState)
        {
            case EnemyStates.Idle:
                //Do nohing basically

                SmoothDecelerate(0f, settings.timeMaxToZero);
                break;
            case EnemyStates.Attack:

                if (!isHurt)
                {
                    SmoothDecelerate(0f, settings.timeMaxToZero);
                    FaceTarget();
                    //Attack player

                    if (canAttack)
                    {
                        if (ClearShot()) FireProjectile();
                    }
                  
                }
               

                break;
            case EnemyStates.Destroy:

                if (!isHurt)
                {

                    FaceTarget();

                    SmoothDecelerate(0f, settings.timeMaxToZero);
                }
                break;

            case EnemyStates.Chase:
                if (!isHurt)
                {

                    FaceMovementDirection(navComp.navAgent.velocity);

                }
                else
                {
                    SmoothDecelerate(0f, settings.timeMaxToZero);
                }
                break;

        }
    }

    protected override void ProcessAI()
    {

        switch (currentState)
        {
            case EnemyStates.Idle:

             
                break;

            case EnemyStates.Attack:
                EvaluateOutOfRange();
                FaceTarget();

                break;

            case EnemyStates.Destroy:
                //Destroy mechanics

                BreakAppliance();
                EvaluateOutOfRange();

                break;

            case EnemyStates.Chase:
                //use navigation
                if (!isHurt)
                {

                    EvaluateInRange();

                }
                break;



        }
    }
    override protected void OnStateChange(EnemyStates newState)
    {
        switch (newState)
        {
            case EnemyStates.Idle:


                navComp.Stop();
                navComp.enabled = false;


                animController.PlayAnim("Shooting");

                aSource.Stop();
                break;
            case EnemyStates.Chase:

                if (target)
                {
                    navComp.enabled = true;
                    navComp.StartAgent(target);
                    animController.PlayAnim("Walking");
                    ResolveTargetType();
                    ChangeSFX("BugsCrawling");
                    aSource.Play();
                }
                else
                {
                    SetEnemyState(EnemyStates.Idle);
                }



                break;
            case EnemyStates.Attack:

                if (navComp.enabled)
                {
                    navComp.Stop();
                    navComp.enabled = false;


                }

                animController.PlayAnim("Shooting");


                break;

           


        }

    }

    private bool ClearShot()
    {
        return true;
    }

    private void FireProjectile()
    {
        
    }

    private void ResetShotTime()
    {
        canAttack = true;
    }
}
