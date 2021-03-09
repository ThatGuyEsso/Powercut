using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
    protected NavMeshAgent navAgent;
    //VFX
    [SerializeField]
    protected GameObject hurtNumber;
    protected MultiSpriteHurtFlash hurtVFX;

    //Pathfinding
    public Transform target;
    protected Vector3 moveDirection;

    //stats
    protected float currentHealth;
    private float smoothAX;
    private float smoothAY;
    private float smoothDX;
    private float smoothDY;

    //VFx
    public GameObject deathVFX;

    virtual protected void Awake()
    {
        //cache component references
        rb = gameObject.GetComponent<Rigidbody2D>();
        currentHealth = settings.maxHealth;
        //cache navigation 
        navAgent = gameObject.GetComponent<NavMeshAgent>();
        hurtVFX = gameObject.GetComponentInChildren<MultiSpriteHurtFlash>();

        //navmesh2D values
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
        //initial values
        currHurtTime = settings.hurtTime;
        currTimeBeforeInvulnerable = settings.timeBeforeInvulnerable;
        BindToInitManager();



        InvokeRepeating("ProcessAi", 0.0f, settings.aiTickrate);

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
      
        float targetAngle = EssoUtility.GetAngleFromVector((navAgent.velocity.normalized));
       /// turn offset -Due to converting between forward vector and up vector
        if (targetAngle < 0) targetAngle += 360f;
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

        //if (inLight)
        //{
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
      
        //}
        

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
    
    protected void ResetInvulnarability()
    {
        if (currTimeBeforeInvulnerable <= 0)
        {
            //inLight = false;
            currTimeBeforeInvulnerable = settings.timeBeforeInvulnerable;

        }
        else
        {
            currTimeBeforeInvulnerable -= Time.deltaTime;
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
        Transform lightTarget = LevelLampsManager.instance.GetNearestFuseLightFuse(transform);
        Transform taskTarget = TaskManager.instance.GetNearestTask(transform);

        //if(Vector2.Distance(transform.position,taskTarget.position) < Vector2.Distance(transform.position, taskTarget.position))
        if (lightTarget != false && taskTarget!=false)
        {
            if (Vector2.Distance(transform.position, taskTarget.position) <= Vector2.Distance(transform.position, taskTarget.position))
            {
                SetTarget(taskTarget);
            }
            else
            {
                SetTarget(lightTarget);
            }
               
        }else if(lightTarget != false && taskTarget == false)
        {
            SetTarget(lightTarget);
        }else if(lightTarget == false && taskTarget != false)
        {
            SetTarget(taskTarget);
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
        //inLight = true;
        //Debug.Log("vulnerable");
    }

    protected void Damage(float damage,Vector2 knockBackDir, float knockBack)
    {
        if (!isHurt)
        {

            if (canBeHurt)
            {
                navAgent.enabled = false;
                canBeHurt = false;
                isHurt = true;
                hurtVFX.BeginFlash();
                currentHealth -= damage;
                this.knockBack = knockBack * knockBackDir;
                DamageNumber dmgVFX = ObjectPoolManager.Spawn(hurtNumber, transform.position, Quaternion.identity).GetComponent<DamageNumber>();
                if(dmgVFX != false)
                {
                    dmgVFX.Init();
                    dmgVFX.SetTextValues(damage, settings.maxHealth, knockBackDir);
                }

                if (currentHealth <= 0)
                {
                    KillEnemy();
                }
               
               

            }
        }
    }
    protected void KillEnemy()
    {
        ObjectPoolManager.Spawn(deathVFX, transform.position, transform.rotation);
        InitStateManager.instance.OnStateChange -= EvaluateNewState;
        if (GameIntensityManager.instance != false) GameIntensityManager.instance.DecrementNumberOfCrawlers();
        ObjectPoolManager.Recycle(gameObject);
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

    public void BindToInitManager()
    {
        InitStateManager.instance.OnStateChange += EvaluateNewState;
    }
    private void EvaluateNewState(InitStates newState)
    {
        switch (newState)
        {
            case InitStates.RespawnPlayer:
                InitStateManager.instance.OnStateChange -= EvaluateNewState;
                ObjectPoolManager.Recycle(gameObject);
            
                break;
            
                case InitStates.PlayerDead:
                    SetEnemyState(EnemyStates.Wander);
                    break;
                case InitStates.LoadTitleScreen:
                ObjectPoolManager.Recycle(gameObject);

                    break;
                case InitStates.ExitLevel:
                ObjectPoolManager.Recycle(gameObject);

                    break;
                
        }
    }

    //check if target is in range
    virtual protected void EvaluateInRange()
    {
        //get distance
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (isTargetHuman)//if human attack if in range
        {
            if (distance <= settings.attackRange)
            {
                SetEnemyState(EnemyStates.Attack);
            }
        }
        else//otherwise it is an object so destroy its
        {
            if (distance <= settings.destroyRange)
            {
                SetEnemyState(EnemyStates.Destroy);
            }
        }
    }

    virtual protected void EvaluateOutOfRange()
    {
        //get distance
        float distance = Vector3.Distance(transform.position, target.transform.position);
        if (isTargetHuman)//if human attack if in range
        {
            if (distance > settings.attackRange)
            {
                SetEnemyState(EnemyStates.Chase);
            }
        }
        else//otherwise it is an object so destroy its
        {
            if (distance >= settings.destroyRange)
            {
                SetEnemyState(EnemyStates.Chase);
            }
        }
    }

    virtual protected void OnDisable()
    {
        InitStateManager.instance.OnStateChange -= EvaluateNewState;
    }
    virtual protected void OnEnable()
    {
        InitStateManager.instance.OnStateChange += EvaluateNewState;
    }


}
