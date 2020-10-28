using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCrawler : BaseEnemy
{
    private float maxDamge;
    private float minDamage;
    private float maxSpeed;

    public float maxScaleMultiplier;
   
    protected override void Awake()
    {
        base.Awake();
        //Set initial variables need to define own variables to change them
        maxDamge = settings.maxDamage;
        minDamage = settings.minDamage;
        maxSpeed = settings.maxSpeed;
        //Initiate mutation of base character
        RandomStatMutation();
    }

    private void Start()
    {
        if (target != null)
        {
            SetEnemyState(EnemyStates.Chase);
            Debug.Log("Chase");
        }
    }
    protected override void ProcessAI()
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
                if (!IsPlayerInAttackRange())
                {
                    SetEnemyState(EnemyStates.Chase);
                }
                break;

            case EnemyStates.Destroy:
                //Destroy mechanics
                BreakAppliance();
                FaceTarget();
                SmoothDecelerate(0f, settings.timeMaxToZero);
                break;

            case EnemyStates.Chase:
                //Move to target position
                ResolveTargetType();
                UpdatePath();
                DrawPathToTarget();
                SmoothAccelerate(moveDirection, maxSpeed, settings.timeZeroToMax);
                FaceMovementDirection();
                break;

            case EnemyStates.Wander:
                //Move around randomly 
                break;
        }
    }

    protected void FixedUpdate()
    {
      
        ProcessAI();
    }

    protected override void DrawPathToTarget()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);

        if (path == null)
        {
            return; //No path so return
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            return; // current waypoint is out of range of total way point. Hence path end has been reached return
        }

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
    
        if (distance < nextWaypointDistance) // if the distance to the next waypoint is shorter than the current one, go to it
        {
            currentWaypoint++; // AI cuts corners
        }
        moveDirection = path.vectorPath[currentWaypoint] - transform.position;



    }
    public void RandomStatMutation()
    {
        float mutationMultipler = Random.Range(1f, maxScaleMultiplier);//Get multplier in range of current scale to max scale
        transform.localScale = new Vector3(transform.localScale.x * mutationMultipler, transform.localScale.y * mutationMultipler, transform.localScale.z * mutationMultipler);//Increase scale by mutation
       
        //scale base stats by mutation
        maxSpeed *= (1-(mutationMultipler-1));

        maxDamge *= mutationMultipler;
        minDamage *= mutationMultipler;
        Debug.Log(maxSpeed);
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
