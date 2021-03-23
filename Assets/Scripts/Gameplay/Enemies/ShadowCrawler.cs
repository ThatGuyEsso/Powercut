using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCrawler : BaseEnemy, IBoid
{
    protected float maxDamage;
    protected float minDamage;
    protected float maxNavSpeed;
    protected float maxSpeed;
  
    [SerializeField] private float chargeSpeed;

    public float maxScaleMultiplier;
    protected BaseEnemyAnimController animController;

    protected override void Awake()
    {
        base.Awake();

        //Set initial variables need to define own variables to change them
        maxDamage = settings.maxDamage;
        minDamage = settings.minDamage;
        maxSpeed = settings.maxMovementSpeed;
        maxNavSpeed = settings.maxNavSpeed;

        navComp.Init();
        movementManager = GetComponent<SteeringManager>();
      

        animController = gameObject.GetComponent<BaseEnemyAnimController>();
        animController.Init();

        //Initiate mutation of base character
        RandomStatMutation();


        if (isSquadLeader)
        {
            navComp.enabled = true;
            movementManager.Init(maxSpeed, false);
            movementManager.enabled = false;
        }
        else
        {
            navComp.enabled = false;
            movementManager.Init(maxSpeed, true);
            movementManager.enabled = true;
        }
    



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
 
                SmoothDecelerate(0f, settings.timeMaxToZero);
                break;

            case EnemyStates.Attack:
                EvaluateOutOfRange();
              
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

            case EnemyStates.Wander:
                //Move around randomly 
                break;
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
                    FaceTarget();
                    //Attack player
                    ChargePlayer();

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
                    if (isSquadLeader)
                        FaceMovementDirection(navComp.navAgent.velocity);
                    else
                        FaceMovementDirection(rb.velocity);
                }
                break;

        }
    }

    override protected void OnStateChange(EnemyStates newState)
    {
        switch (newState)
        {
            case EnemyStates.Idle:
                if (isSquadLeader)
                {
                    navComp.Stop();
                    navComp.enabled = false;

                }
                animController.PlayAnim("Idle");

                break;
            case EnemyStates.Chase:
                if (isSquadLeader)
                {
                    if (target)
                    {
                        navComp.enabled = true;
                        navComp.StartAgent(target);
                     
                    }
                }
                else
                {
                    movementManager.BeginFollowLeader(leader,settings.attackRange);
                }
                animController.PlayAnim("Walk");
                ResolveTargetType();
         
                break;
            case EnemyStates.Attack:
                if (isSquadLeader)
                { 
                        navComp.Stop();
                        navComp.enabled = false;
                    animController.PlayAnim("Walk");

                }
                else
                {
                    movementManager.DeactivateAll();
                }
                break;
           
            case EnemyStates.Destroy:
                if (isSquadLeader)
                {
                    navComp.Stop();
                    navComp.enabled = false;

                }
                else
                {
                    movementManager.DeactivateAll();
                }
                animController.PlayAnim("Break");
                break;
        }
    }


    public void RandomStatMutation()
    {
        float mutationMultipler = Random.Range(1f, maxScaleMultiplier);//Get multplier in range of current scale to max scale
        transform.localScale = new Vector3(transform.localScale.x,
        transform.localScale.y ,
        transform.localScale.z)*mutationMultipler;//Increase scale by mutation
       

        //scale base stats by mutation
        maxNavSpeed *= (1-(mutationMultipler-1));
        maxSpeed *= (1 - (mutationMultipler - 1));
        chargeSpeed *= ((mutationMultipler-1));
        navComp.navAgent.speed = maxNavSpeed;
        maxDamage *= mutationMultipler;
        minDamage *= mutationMultipler;

    }

    public void ChargePlayer()
    {
        moveDirection = target.position - transform.position;
        SmoothAccelerate(moveDirection, chargeSpeed, settings.timeZeroToMax);
    }

    override protected void BreakAppliance()
    {
        if (canDestroy)
        {
            canDestroy = false;
            float dmg = Random.Range(minDamage,maxDamage);
            IBreakable appliance = target.GetComponent<IBreakable>();
            if (appliance != null)
            {

                appliance.Damage(dmg,this);
            }
        }
    }

    public Transform GetTarget()
    {
        return target;
    }

    public Vector2 GetPosition()
    {
        return rb.position;
    }
    public Vector2 GetVelocity()
    {
        return rb.velocity;
    }

    public IBoid GetLeader()
    {
        return leader;
    }

    public float GetArrivalRadius()
    {
        return settings.attackRange;
    }

    public SteeringManager GetMovementManager()
    {
        return movementManager;
    }

    public float GetRadius()
    {
        return settings.followRadius;
    }

    public float GetSightLength()
    {
        return settings.sightLength;
    }

    public float GetBehindLength()
    {
        return settings.behindLength;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public Vector3 GeRightVector()
    {
        return transform.right;
    }
}
