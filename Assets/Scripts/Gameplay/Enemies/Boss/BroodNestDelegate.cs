using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroodNestDelegate : MonoBehaviour, IHurtable
{
    [SerializeField] private ChargingCable teether;
    [SerializeField] private float hurtTime;
    [SerializeField] private Collider2D sphereCollider;
    [SerializeField] private SpriteRenderer gfx;
    [SerializeField] private SpriteFlash hurtVFX;
    [SerializeField] private GameObject deathVFX;
    [SerializeField] private GameObject spawnVFX;
    [SerializeField] private GameObject audioPlayerPrefab;
    [SerializeField] private GameObject hurtNumber;
    private BroodNest hive;
    private bool isHurt;

    [Header("Settings")]
    [SerializeField] private float maxAttackRate;
    [SerializeField] private float minAttackRate;
    [SerializeField] private float closeRange;
    [SerializeField] private float attackRange;
    [SerializeField] private float maxHealth;
    private float currHealth;

    [Header("Abilities")]
    [SerializeField] private AttackPatternData droneData;
    [SerializeField] private AttackPatternData pheremoneData;

    [SerializeField] private PheremoneBlast pheremones;
    [SerializeField] private AttackDrones drones;

    private Transform playerTransform = null;


    public void SetUpBroodNest(BroodNest hiveRef)
    {
        hive = hiveRef;

        currHealth = maxHealth;
        EnableBroodDelegate();
        isHurt = false;
        IAudio player = ObjectPoolManager.Spawn(audioPlayerPrefab.gameObject, transform.position, transform.rotation).GetComponent<IAudio>();
        player.SetUpAudioSource(AudioManager.instance.GetSound("BugsSplat"));
        player.PlayAtRandomPitch();
        ObjectPoolManager.Spawn(spawnVFX, transform.position, transform.rotation);
        CreateTeether();
        drones.SetUpAbilityData(droneData);
        playerTransform = GameStateManager.instance.GetPlayerTransform();
        if (!playerTransform) playerTransform = FindObjectOfType<PlayerBehaviour>().transform;
        drones.playerTransform = playerTransform;
        pheremones.SetUpAbilityData(pheremoneData);
        StartCoroutine(AttackLoop());
    }

    public void Damage(float damage, Vector3 knockBackDir, float knockBack)
    {
        if (!isHurt)
        {
            isHurt = true;
            hurtVFX.BeginFlash();
            currHealth -= damage;
            ObjectPoolManager.Spawn(deathVFX, transform.position, transform.rotation);
            IAudio player = ObjectPoolManager.Spawn(audioPlayerPrefab.gameObject, transform.position, transform.rotation).GetComponent<IAudio>();
            player.SetUpAudioSource(AudioManager.instance.GetSound("BugsSplat"));
            player.PlayAtRandomPitch();
            DamageNumber dmgVFX = ObjectPoolManager.Spawn(hurtNumber, transform.position, Quaternion.identity).GetComponent<DamageNumber>();
            if (dmgVFX != false)
            {
                dmgVFX.Init();
                dmgVFX.SetTextValuesAtScale(damage, maxHealth, knockBackDir, 2f);
            }
            if (currHealth <= 0.0f)
            {
                DisableBroodDelegate();
            }
            else
            {
                Invoke("ResetHurt", hurtTime);
            }

        }
    }
    private void CreateTeether()
    {
        teether.SetOrigin(hive.transform);
        teether.StartDrawingRope(transform);
    }

    private void EndTether()
    {
        teether.StopDrawingRope();

    }

    public IEnumerator AttackLoop()
    {
        float randRate = Random.Range(minAttackRate, maxAttackRate);
        yield return new WaitForSeconds(randRate);
        ProcessAttack();
        StartCoroutine(AttackLoop());
    }
    private void ProcessAttack()
    {
        if (playerTransform)
        {
            float distance = Vector2.Distance(transform.position, playerTransform.position);
            if (distance <= attackRange && distance > closeRange)
            {
                drones.ExecuteAttack();
            }
            else if (distance <= closeRange)
            {
                pheremones.ExecuteAttack();
            }
        }
            
    }

    public void Push(Vector3 knockBackDir, float knockBack)
    {
        throw new System.NotImplementedException();
    }


    public void DisableBroodDelegate()
    {
        gfx.enabled = false;
        sphereCollider.enabled = false;
        EndTether();
        hive.DecrementBroodDelegateCount(this);
        drones.StopRunning();
        pheremones.StopRunning();
        StopAllCoroutines();

    }
public void EnableBroodDelegate()
    {
        gfx.enabled = true;
        sphereCollider.enabled = true;
    }

    private void ResetHurt()
    {
        isHurt = false;
        hurtVFX.EndFlash();
    }
}
