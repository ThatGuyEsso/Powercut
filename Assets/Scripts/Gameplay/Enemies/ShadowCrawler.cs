using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCrawler : BaseEnemy
{
    private float maxDamage;
    private float minDamage;
    private float maxSpeed;
    [SerializeField] private float chargeSpeed;

    public float maxScaleMultiplier;
    private BaseEnemyAnimController animController;

    protected override void Awake()
    {
        base.Awake();

        //Set initial variables need to define own variables to change them
        maxDamage = settings.maxDamage;
        minDamage = settings.minDamage;
        maxSpeed = settings.maxSpeed;


        navComp.Init();
        if (isSquadLeader) navComp.enabled = true;
        else navComp.enabled = false;

        animController = gameObject.GetComponent<BaseEnemyAnimController>();
        animController.Init();

        //Initiate mutation of base character
        RandomStatMutation();

  

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
                    FaceMovementDirection();
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
                break;
           
            case EnemyStates.Destroy:
                if (isSquadLeader)
                {
                    navComp.Stop();
                    navComp.enabled = false;

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
        maxSpeed *= (1-(mutationMultipler-1));
        chargeSpeed *= ((mutationMultipler-1));
        navComp.navAgent.speed = maxSpeed;
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
                Debug.Log("Attacking");
                appliance.Damage(dmg,this);
            }
        }
    }

}
