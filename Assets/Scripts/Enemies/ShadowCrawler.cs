using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCrawler : BaseEnemy
{
    private float maxDamge;
    private float minDamage;
    private float maxSpeed;

    public float maxScaleMultiplier;
    private BaseEnemyAnimController animController;

    protected override void Awake()
    {
        base.Awake();

        //Set initial variables need to define own variables to change them
        maxDamge = settings.maxDamage;
        minDamage = settings.minDamage;
        maxSpeed = settings.maxSpeed;
        //Initiate mutation of base character
        RandomStatMutation();
        animController = gameObject.GetComponent<BaseEnemyAnimController>();
        animController.Init();

    }

    private void Start()
    {
        if (target != null)
        {
            SetEnemyState(EnemyStates.Chase);
          
        }
    }
    protected override void ProcessAI()
    {
        switch (currentState)
        {
            case EnemyStates.Idle:
                //Do nohing basically
                animController.PlayAnim("Idle");
                SmoothDecelerate(0f, settings.timeMaxToZero);
                break;

            case EnemyStates.Attack:
                //Don't use navigation when charging
                navAgent.enabled = false;


                animController.PlayAnim("Walk");
                EvaluateOutOfRange();
                break;

            case EnemyStates.Destroy:
                //Destroy mechanics
                animController.PlayAnim("Break");
                BreakAppliance();
                EvaluateOutOfRange();
                break;

            case EnemyStates.Chase:
                //use navigation
                if (!isHurt)
                {
                    navAgent.enabled = true;
                    EvaluateInRange();
                    if (target != null && navAgent.enabled)
                    {
                        navAgent.SetDestination(target.position);
                    }
                    //check if in range


                    //Move to target position
                    ResolveTargetType();
                    animController.PlayAnim("Walk");
                }
       
              
                break;

            case EnemyStates.Wander:
                //Move around randomly 
                break;
        }
    }
    protected override void Update()
    {
        base.Update();

    }
    protected void FixedUpdate()
    {


        switch (currentState)
        {
            case EnemyStates.Idle:
                //Do nohing basically

                SmoothDecelerate(0f, settings.timeMaxToZero);
                break;
            case EnemyStates.Attack:

                if (!isHurt)
                {
                    //Attack player
                    ChargePlayer();
                    FaceTarget();

                }

                break;
            case EnemyStates.Destroy:

                FaceTarget();
                SmoothDecelerate(0f, settings.timeMaxToZero);
                break;

            case EnemyStates.Chase:
                //Move

                //SmoothAccelerate(moveDirection, maxSpeed, settings.timeZeroToMax);
                FaceMovementDirection();
                break;

        }
    }


    
    public void RandomStatMutation()
    {
        float mutationMultipler = Random.Range(1f, maxScaleMultiplier);//Get multplier in range of current scale to max scale
        transform.localScale = new Vector3(transform.localScale.x * mutationMultipler,
        transform.localScale.y * mutationMultipler,
        transform.localScale.z * mutationMultipler);//Increase scale by mutation
       
        //scale base stats by mutation
        maxSpeed *= (1-(mutationMultipler-1));
        navAgent.speed = maxSpeed *settings.navAgentSpeedScalar;
        maxDamge *= mutationMultipler;
        minDamage *= mutationMultipler;

    }

    public void ChargePlayer()
    {
        moveDirection = target.position - transform.position;
        SmoothAccelerate(moveDirection, maxSpeed, settings.timeZeroToMax);
    }



    override protected void BreakAppliance()
    {
        if (canDestroy)
        {
            canDestroy = false;
            float dmg = Random.Range(minDamage,maxDamge);
            IBreakable appliance = target.GetComponent<IBreakable>();
            if (appliance != null)
            {
                appliance.Damage(dmg,this);
            }
        }
    }

}
