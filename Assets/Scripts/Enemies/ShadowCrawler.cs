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
        InvokeRepeating("ProcessAI", 0f, settings.aiTickrate);
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

                animController.PlayAnim("Walk");
                if (!IsPlayerInAttackRange())
                {
                    SetEnemyState(EnemyStates.Chase);
                }
                break;

            case EnemyStates.Destroy:
                //Destroy mechanics
                animController.PlayAnim("Break");
                BreakAppliance();

                break;

            case EnemyStates.Chase:
                UpdatePath();
                DrawPathToTarget();

                //Move to target position
                ResolveTargetType();
                animController.PlayAnim("Walk");

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
                //Attack player
                ChargePlayer();

                FaceTarget();

                break;
            case EnemyStates.Destroy:
                FaceTarget();
                SmoothDecelerate(0f, settings.timeMaxToZero);
                break;

            case EnemyStates.Chase:
                //Move to target position
            



                SmoothAccelerate(moveDirection, maxSpeed, settings.timeZeroToMax);
                FaceMovementDirection();
                break;

        }
    }

    protected override void DrawPathToTarget()
    {
        //valid path checks
        if (target == false) return;
        if (path == null) return; //No path so return
        if (currentWaypoint >= path.vectorPath.Count) return;

        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (isTargetHuman)
        {
            if (distance <= settings.attackRange)
            {
                SetEnemyState(EnemyStates.Attack);
            }
        }
        else
        {
            if (distance <= settings.destroyRange)
            {
                SetEnemyState(EnemyStates.Destroy);
            }
        }

        distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        moveDirection = path.vectorPath[currentWaypoint] - (Vector3)rb.position;
        if (distance < nextWaypointDistance) // if the distance to the next waypoint is shorter than the current one, go to it
        {
            currentWaypoint++; // AI cuts corners
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

        maxDamge *= mutationMultipler;
        minDamage *= mutationMultipler;

    }

    public void ChargePlayer()
    {
        moveDirection = target.position - transform.position;
        SmoothAccelerate(moveDirection, maxSpeed, settings.timeZeroToMax);
    }
    public bool IsPlayerInAttackRange()
    {

        return Vector2.Distance(transform.position, target.position) <= settings.attackRange;
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
