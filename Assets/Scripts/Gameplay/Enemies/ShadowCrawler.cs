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

                  
                    //set a new destination if it can
                    if (target != false && navAgent.enabled)
                    {
                        navAgent.SetDestination(target.position);
                    }
                  
                    //if it has a path
                    else
                    {
                        //check if it actually has a yatget
                        if (target == false && navAgent.enabled)
                        {
                            navAgent.ResetPath();//if it doesn't have a path, clear it 
                         
                        }
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

                FaceTarget();
                SmoothDecelerate(0f, settings.timeMaxToZero);
                break;

            case EnemyStates.Chase:

                FaceMovementDirection();
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
        navAgent.speed = maxSpeed;
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

}
