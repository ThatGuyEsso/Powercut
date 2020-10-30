using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum EnemyStates
{
    Wander,
    Chase,
    Attack,
    Destroy,
    Idle
};
[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseEnemy : MonoBehaviour, IBreakable, IHurtable, ILightWeakness
{
    //States
    protected EnemyStates currentState;
    protected bool isHurt;
    protected bool isTargetHuman;
    protected bool canDestroy;
    protected bool canBeHurt;
    protected bool inLight;
    //Timers
    protected float currTimeToDestroy;
    protected float currTimeToAttack;
    protected float currHurtTime;
    protected float currTimeBeforeInvulnerable;
    //Settings
    public EnemySettings settings;
    protected float smoothRot;
    protected Vector2 knockBack;
    //Component refs
    protected Rigidbody2D rb;
    protected HurtFlash hurtVFX;

    //Pathfinding
    protected int currentWaypoint = 0;
    public Transform target;
    protected Path path;
    protected Seeker seeker;
    protected Vector3 moveDirection;
    protected float nextWaypointDistance = 3f;
    //stats
    protected float currentHealth;
    private float smoothAX;
    private float smoothAY;
    private float smoothDX;
    private float smoothDY;

    virtual protected void Awake()
    {
        //cache component references
        rb = gameObject.GetComponent<Rigidbody2D>();
        currentHealth = settings.maxHealth;
        seeker = gameObject.GetComponent<Seeker>();
        hurtVFX = gameObject.GetComponentInChildren<HurtFlash>();
        currHurtTime = settings.hurtTime;
        currTimeBeforeInvulnerable = settings.timeBeforeInvulnerable;
       // InvokeRepeating("ProcessAI", 0f, settings.aiTickrate);
    }




    virtual protected void ProcessAI()
    {
        //Require own AI implementation
    }
    //#MovementFunction FUNCTIONS#
    virtual protected void SmoothAccelerate(Vector3 direction, float maxSpeed, float rate)
    {
        Vector2 targetVelocity = Vector2.zero;

        targetVelocity.x = Mathf.SmoothDamp(rb.velocity.x, maxSpeed * direction.x, ref smoothAX, rate);
        targetVelocity.y = Mathf.SmoothDamp(rb.velocity.y, maxSpeed * direction.y, ref smoothAY, rate);

        CalculateKnockBack();
        rb.velocity = targetVelocity*Time.deltaTime+ knockBack;
    }

    virtual protected void SmoothDecelerate(float minSpeed, float rate)
    {
        Vector2 targetVelocity = Vector2.zero;

        if (rb.velocity.magnitude <= 0.1f)
        {
            smoothAX = 0f;
            smoothAY = 0f;
            smoothDX = 0f;
            smoothDY = 0f;
        }
        else
        {
            targetVelocity.x = Mathf.SmoothDamp(rb.velocity.x, minSpeed, ref smoothDX, rate);
            targetVelocity.y = Mathf.SmoothDamp(rb.velocity.y, minSpeed, ref smoothDY, rate);
        }
        CalculateKnockBack();
        rb.velocity = targetVelocity + knockBack;
    }

    virtual protected void FaceMovementDirection()
    {
        float targetAngle = EssoUtility.GetAngleFromVector(rb.velocity.normalized);
       /* targetAngle += 90f;*/// turn offset -Due to converting between forward vector and up vector
        //if (targetAngle < 0) targetAngle += 360f;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle, ref smoothRot, settings.rotationSpeed);//rotate player smoothly to target angle
        transform.rotation = Quaternion.Euler(0f, 0f, angle);//update angle
        //fovObject.SetAimDirection((-1)*fovObject.GetVectorFromAngle(angle));
    }

    protected void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (currentState == EnemyStates.Attack)
            {
                float dmg = Random.Range(settings.minDamage, settings.maxDamage);
                float knockBack = Random.Range(settings.minKnockBack, settings.maxKnockBack);
                other.gameObject.GetComponent<IHurtable>().Damage(dmg, rb.velocity.normalized, knockBack);

            }
        }
    }
    protected void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (currentState == EnemyStates.Attack)
            {
                float dmg = Random.Range(settings.minDamage, settings.maxDamage);
                float knockBack = Random.Range(settings.minKnockBack, settings.maxKnockBack);
                other.gameObject.GetComponent<IHurtable>().Damage(dmg, rb.velocity.normalized, knockBack);
            }

        }
    }
    virtual protected void FaceTarget()
    {
        if (target != null)
        {
            float targetAngle = EssoUtility.GetAngleFromVector((target.position-transform.position).normalized);
            /* targetAngle += 90f;*/// turn offset -Due to converting between forward vector and up vector
                                    //if (targetAngle < 0) targetAngle += 360f;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle, ref smoothRot, settings.rotationSpeed);//rotate player smoothly to target angle
            transform.rotation = Quaternion.Euler(0f, 0f, angle);//update angle
            //fovObject.SetAimDirection((-1)*fovObject.GetVectorFromAngle(angle));

        }
    }
    //#End MovementFunction FUNCTIONS#




    //#PATHFINDING FUNCTIONS#
    virtual protected void UpdatePath()
    {
        if (target != null)
        {
            if (seeker.IsDone()) //if seeker is done mapping, start the path to target
                seeker.StartPath(rb.position, target.position, OnPathComplete);
        }

    }
    virtual protected void OnPathComplete(Path p)
    {
        if (!p.error)  //if no errors, reset path and waypoint
        {
            path = p;
            currentWaypoint = 0;
        }

    }

    virtual protected void Update()
    {
        
        if (!canDestroy)
        {
            if(currTimeToDestroy <= 0)
            {
                canDestroy = true;
                currTimeToDestroy = settings.destroyRate;
            }
            else
            {
                currTimeToDestroy -= Time.deltaTime;
            }
        }

        if (inLight)
        {
            if (!canBeHurt)
            {
                if (currHurtTime <= 0)
                {
                    canBeHurt = true;
                    currHurtTime = settings.hurtTime;
                }
                else
                {
                    currHurtTime -= Time.deltaTime;
                }
            }
            if (currTimeBeforeInvulnerable <= 0)
            {
                inLight = false;
                currTimeBeforeInvulnerable = settings.timeBeforeInvulnerable;

            }
            else
            {
                currTimeBeforeInvulnerable -= Time.deltaTime;
            }
        }
        

        if (isHurt)
        {
            if (currHurtTime <= 0)
            {
                currHurtTime = settings.hurtTime;
                isHurt = false;
                hurtVFX.EndFlash();

            }
            else
            {
                currHurtTime -= Time.deltaTime;
            }
        }
    }



    virtual protected void DrawPathToTarget()
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

            distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance) // if the distance to the next waypoint is shorter than the current one, go to it
            {
                currentWaypoint++; // AI cuts corners
            }
            moveDirection = path.vectorPath[currentWaypoint] - transform.position;

    }
    //# END OF PATHFINDING FUNCTIONS#


    //#Setters#
    //Set target of enemy
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    //Set state 
    public void SetEnemyState(EnemyStates newState)
    {
        currentState = newState;
    }
    //# End of Setters#

    protected void ResolveTargetType()
    {
        if(target != null)
        {
            if (target.CompareTag("Player"))
            {
                isTargetHuman = true;
            }
            else
            {
                isTargetHuman = false;
            }

        }
    }
    
    void IBreakable.Damage(float damage, BaseEnemy interfacingEnemy)
    {
        //Does not matter to enemies just allows them to interface with lamps
    }

    virtual protected void BreakAppliance()
    {
        if (canDestroy)
        {
            canDestroy = false;
            float dmg = Random.Range(settings.minDamage, settings.maxDamage);
            LightFuse fuse = target.GetComponent<LightFuse>();
            if (fuse != null)
            {
                fuse.GetComponent<IBreakable>().Damage(dmg,this);
            }
        }
    }

    void IBreakable.ObjectIsBroken()
    {
        Transform newTarget = LevelLampsManager.instance.GetNearestFuseLightFuse(transform);
        if (newTarget != null)
        {
            SetTarget(newTarget);
        }
        else
        {
            target = FindObjectOfType<PlayerBehaviour>().transform;
        }
        SetEnemyState(EnemyStates.Chase);
    }

    void IHurtable.Damage(float damage, Vector3 knockBackDir, float knockBack)
    {
        Damage(damage, knockBackDir, knockBack);
        SetTarget(FindObjectOfType<PlayerBehaviour>().transform);
        SetEnemyState(EnemyStates.Chase);
    }

    void ILightWeakness.MakeVulnerable()
    {
        inLight = true;
        Debug.Log("vulnerable");
    }

    protected void Damage(float damage,Vector2 knockBackDir, float knockBack)
    {
        if (!isHurt)
        {

            if (canBeHurt)
            {
                canBeHurt = false;
                isHurt = true;
                hurtVFX.BeginFlash();
                currentHealth -= damage;
                this.knockBack = knockBack * knockBackDir;
                if(currentHealth <= 0)
                {
                    KillEnemy();
                }
               
               

            }
        }
    }
    protected void KillEnemy()
    {
        Destroy(gameObject);
    }

    public void CalculateKnockBack()
    {
        if (knockBack.magnitude > 0)
        {
            //rb.velocity = knockBack;
            knockBack = Vector2.Lerp(knockBack, Vector2.zero, settings.knockBackFallOff);
            if (knockBack.magnitude < 0.5f)
            {
                knockBack = Vector2.zero;
            }
        }
    }
}
