using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCrawler : BaseEnemy
{
    protected float maxDamage;
    protected float minDamage;
    protected float maxNavSpeed;
    protected float maxSpeed;
  

    [SerializeField] private float chargeSpeed;

    [SerializeField] private float defaultScale;
    public float maxScaleMultiplier;
  
 
    protected override void Awake()
    {
        base.Awake();
       
        //Set initial variables need to define own variables to change them
        maxDamage = settings.maxDamage;
        minDamage = settings.minDamage;
        maxSpeed = settings.maxMovementSpeed;
        maxNavSpeed = settings.maxNavSpeed;

        navComp.Init();

        animController = gameObject.GetComponent<BaseEnemyAnimController>();
        animController.Init();

        //Initiate mutation of base character
        RandomStatMutation();

    }

    private void Start()
    {
        if (target!=false)
        {
            SetEnemyState(EnemyStates.Chase);
            navComp.enabled = true;
            }
        else
        {
                navComp.enabled = false;
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

                    SmoothDecelerate(0f, settings.timeMaxToZero);
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

                    SmoothDecelerate(0f, settings.timeMaxToZero);
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

           
                animController.PlayAnim("Idle");
            
                aSource.Stop();
                break;
            case EnemyStates.Chase:

                if (target)
                {
                    navComp.enabled = true;
                    navComp.StartAgent(target);
                    animController.PlayAnim("Walk");
                    ResolveTargetType();
                    ChangeSFX("BugsCrawling");
                    if (aSource.enabled)
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
            
                animController.PlayAnim("Walk");
            

                break;

            case EnemyStates.Destroy:
        

                if (navComp.enabled)
                {
                    navComp.Stop();
                    navComp.enabled = false;


                }

                aSource.Stop();
                animController.PlayAnim("Break");
                break;

    
        }
       
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        RandomStatMutation();
    }
    public void RandomStatMutation()
    {
        float mutationMultipler = Random.Range(1f, maxScaleMultiplier);//Get multplier in range of current scale to max scale
        transform.localScale = new Vector3(defaultScale,
       defaultScale,
        defaultScale) *mutationMultipler;//Increase scale by mutation
       

        //scale base stats by mutation
        maxNavSpeed *= (1-(mutationMultipler-1));
        //maxSpeed *= (1 - (mutationMultipler - 1));
        chargeSpeed *= ((mutationMultipler-1));
        navComp.navAgent.speed = maxNavSpeed;
        maxDamage *= mutationMultipler;
        minDamage *= mutationMultipler;

    }

    public void ChargePlayer()
    {
        if (target)
        {
            moveDirection = target.position - transform.position;
            SmoothAccelerate(moveDirection, chargeSpeed, settings.timeZeroToMax);
        }

    }

    override protected void BreakAppliance()
    {
        if (canDestroy&&target)
        {
            canDestroy = false;
            float dmg = Random.Range(minDamage,maxDamage);
            IBreakable appliance = target.GetComponent<IBreakable>();
            if (appliance != null)
            {

                appliance.Damage(dmg,this);
            }
        }
        else if(!target)
        {
            ObjectIsBroken();
        }
    }

    public override void ObjectIsBroken()
    {
        base.ObjectIsBroken();
    }
   
}
