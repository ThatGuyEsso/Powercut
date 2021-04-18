using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroodSpitter : BaseEnemy
{
    [SerializeField] protected LayerMask blockingLayers;
    [SerializeField] protected Transform firePoint;
    protected bool canAttack = true;
    [SerializeField] protected float viewDistance = 10.0f;
    [SerializeField] protected float shootForce = 10.0f;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected List<GameObject> slimeFragmentsPrefabs = new List<GameObject>();

    [SerializeField] protected int fragmentCounts;
    protected override void Awake()
    {
        base.Awake();

 
        navComp.Init();

        animController = gameObject.GetComponent<BaseEnemyAnimController>();
        animController.Init();
        canAttack = false;

        Invoke("ResetShotTime", settings.attackRate);


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
                    
                    FaceTarget();
                    //Attack player

                    if (canAttack)
                    {
                        if (ClearShot()) FireProjectile();
                        else {
                            canAttack = false;
                            Invoke("ResetShotTime", settings.attackRate);
                        }
                    }
                  
                }
                SmoothDecelerate(0f, settings.timeMaxToZero);

                break;
            case EnemyStates.Destroy:

                if (!isHurt)
                {

                    FaceTarget();

                    
                }
                SmoothDecelerate(0f, settings.timeMaxToZero);
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

    override protected void KillEnemy()
    {


        ObjectPoolManager.Spawn(deathVFX, transform.position, transform.rotation);
        IAudio player = ObjectPoolManager.Spawn(audioPlayerPrefab.gameObject, transform.position, transform.rotation).GetComponent<IAudio>();
        player.SetUpAudioSource(AudioManager.instance.GetSound("BugsSplat"));
        player.PlayAtRandomPitch();
        InitStateManager.instance.OnStateChange -= EvaluateNewState;
        if (GameIntensityManager.instance != false) GameIntensityManager.instance.DecrementNumberOfCrawlers();

        if (navComp.enabled)
        {
            navComp.Stop();
            navComp.enabled = false;
        }
        aSource.Stop();
        SpawnFragments();

        Killed?.Invoke(this);
        ObjectPoolManager.Recycle(gameObject);
    }
    override protected void OnStateChange(EnemyStates newState)
    {
        switch (newState)
        {
            case EnemyStates.Idle:

                if (navComp.enabled)
                {
                    navComp.Stop();
                    navComp.enabled = false;


                }
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

                animController.PlayAnim("Shooting");


                break;

           


        }

    }

    override protected void OnCollisionEnter2D(Collision2D other)
    {
       
    }
    override protected void OnCollisionStay2D(Collision2D other)
    {
     
    }
    private bool ClearShot()
    {
        if (target)
        {
  
            RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, transform.right, viewDistance, blockingLayers);
            if (hitInfo)
            {
                return (hitInfo.collider.CompareTag("Player"));
            }
        }
           
        return false;
    }

    private void FireProjectile()
    {
        GameObject bullet = ObjectPoolManager.Spawn(projectilePrefab, firePoint.position, firePoint.rotation);
        IShootable shot = bullet.GetComponent<IShootable>();
    
        IAudio player = ObjectPoolManager.Spawn(audioPlayerPrefab.gameObject, transform.position, transform.rotation).GetComponent<IAudio>();
        player.SetUpAudioSource(AudioManager.instance.GetSound("SlimeFireProjectile"));
        player.PlayAtRandomPitch();
        float dmg = Random.Range(settings.minDamage, settings.maxDamage);
        float kBack = Random.Range(settings.minKnockBack, settings.minKnockBack);
        shot.SetUpBullet(kBack, dmg);
        shot.Shoot(firePoint.up, shootForce);
        canAttack = false;
        Invoke("ResetShotTime", Random.Range(Mathf.Clamp(settings.attackRate-1.5f,0f, settings.attackRate), settings.attackRate+1.5f));
    }

    private void ResetShotTime()
    {

        canAttack = true;
    }

    override public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        ResolveTargetType();
        if (!isTargetHuman)
        {
            target = FindObjectOfType<PlayerBehaviour>().transform;
        }
    
        OnStateChange(currentState);
    }

    public void SpawnFragments()
    {
        float angleIncrement = 360f / fragmentCounts;
        float currentAngle = 0f;
        GameObject currentFragment;
        float dmg = Random.Range(settings.minDamage, settings.maxDamage);
        float kBack = Random.Range(settings.minKnockBack, settings.minKnockBack);
        for (int i = 0; i < fragmentCounts; i++)
        {
            int rand = Random.Range(0, slimeFragmentsPrefabs.Count);
            currentFragment = ObjectPoolManager.Spawn(slimeFragmentsPrefabs[rand], transform.position);

            Vector3 dir = EssoUtility.GetVectorFromAngle(currentAngle).normalized;
            currentFragment.transform.up = dir;
            IShootable frag = currentFragment.GetComponent<IShootable>();
            frag.SetUpBullet(kBack / fragmentCounts, dmg / fragmentCounts);
            frag.Shoot(dir, shootForce * 0.8f);
            currentAngle += angleIncrement;
        }
    }
}
