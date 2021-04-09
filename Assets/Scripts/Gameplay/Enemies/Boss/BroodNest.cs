using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum BossStage
{
    First,
    Second,
    Third,
    Final,
    Transition,
    Inactive
};



public class BroodNest : MonoBehaviour, IInitialisable,IHurtable
{
    [Header("Boss AI Settings")]
    [SerializeField] private float percentNextStageTrigger;
    [SerializeField] private float tickRate;

    [Header("Boss State")]
    private BossStage currentStage;
    private bool isHurt;
    private int currCycleIndex;
    [Header("Settings")]
    [SerializeField] private float MaxHealth;
    private float currHealth;
    [SerializeField] private float hurtTime;

    [SerializeField] private List<BossStageData> stageDatas;
    [Header("Boss Components")]
    [SerializeField] private List<BroodNestDelegate> broodDelegates = new List<BroodNestDelegate>();
    [SerializeField] private ScalingProgressBar healthBar;
    [SerializeField] private BossAnimController healthBarAnim;
    [SerializeField] private SpriteFlash hurtVFX;

    [Header("Boss Abilities")]
    [SerializeField] private PheremoneBlast pheremoneBlast;
    [SerializeField] private SplitAndProtect splitAndProtect;
    [SerializeField] private AttackDrones attackDrones;
    [SerializeField] private SendSoldiers sendSoldiers;
    private List<BaseAttackPattern> currentAttackCycle = new List<BaseAttackPattern>();
    [SerializeField] protected AudioPlayer audioPlayerPrefab;

    //VFX
    [SerializeField] private GameObject hurtNumber;
    [SerializeField] private GameObject deathVFX;

    //Boss references
    [SerializeField] private Transform playerTransform;
    public void Damage(float damage, Vector3 knockBackDir, float knockBack)
    {
        if(currentStage!= BossStage.Transition)
        {
            if (!isHurt)
            {
                hurtVFX.BeginFlash();
                ObjectPoolManager.Spawn(deathVFX, transform.position, transform.rotation);
                IAudio player = ObjectPoolManager.Spawn(audioPlayerPrefab.gameObject, transform.position, transform.rotation).GetComponent<IAudio>();
                player.SetUpAudioSource(AudioManager.instance.GetSound("BugsSplat"));
                player.PlayAtRandomPitch();
                isHurt = true;
                currHealth -= damage;
                if (currHealth < 0f) currHealth = 0f;
                DamageNumber dmgVFX = ObjectPoolManager.Spawn(hurtNumber, transform.position, Quaternion.identity).GetComponent<DamageNumber>();
                if (dmgVFX != false)
                {
                    dmgVFX.Init();
                    dmgVFX.SetTextValuesAtScale(damage, MaxHealth, knockBackDir,10);
                }
                healthBar.UpdateValue(currHealth);
                Invoke("ResetHurt", hurtTime);

            }
        }
    }
    private void Awake()
    {
        Init();
    }
    public void Init()
    {
        currHealth = MaxHealth;
        healthBar.SetMaxValue(MaxHealth);
        healthBar.UpdateValue(currHealth);
        healthBar.HideBar();
        healthBar.gameObject.SetActive(false);
        foreach(BroodNestDelegate nest in broodDelegates)
        {
            nest.gameObject.SetActive(false);
        }
      

    
 
    }

    private void ResetHurt()
    {
        isHurt = false;
        hurtVFX.EndFlash();
    }
    public void InitBossBattle()
    {
        healthBar.gameObject.SetActive(true);
    
        healthBarAnim.animEnded += InitiaionComplete;
        healthBarAnim.PlayAnim("InitiateHealth");
        healthBar.ShowBar();
        BindToGameManager();
        pheremoneBlast.ExecuteAttack();

        playerTransform = GameStateManager.instance.GetPlayerTransform();
        if (playerTransform) playerTransform = FindObjectOfType<PlayerBehaviour>().transform;
    }
    public void StartBossBattle()
    {
        currentStage = BossStage.Second;
        BeginNewStage(currentStage);
        sendSoldiers.BeginAttackPattern();
    }

    public void OnNewBossStage()
    {
        switch (currentStage)
        {
            case BossStage.First:
                BeginNewStage(currentStage);

                break;
            case BossStage.Second:
                BeginNewStage(currentStage);
                break;
            case BossStage.Third:
                BeginNewStage(currentStage);
                break;
            case BossStage.Final:
                BeginNewStage(currentStage);
                break;
            case BossStage.Transition:
                BeginNewStage(currentStage);
                break;
        }
    }
    
    public void Push(Vector3 knockBackDir, float knockBack)
    {
        throw new System.NotImplementedException();
    }

    private void InitiaionComplete()
    {
        CameraManager.instance.ReturnCutScene();
        healthBarAnim.animEnded -= InitiaionComplete;
    }


    public void BindToGameManager()
    {
        GameStateManager.instance.OnGameStateChange += EvaluateNewState;
    }

    private void NextAttackPattern()
    {
        currCycleIndex++;

        if (currCycleIndex >= currentAttackCycle.Count) currCycleIndex = 0;
    }

    public void BeginNewStage(BossStage stage)
    {
        BossStageData stageData = GetStageData(stage);

        if (stageData != null)
        {
            stageData.SetUpData();

            foreach(AttackPatternData attackData in stageData.AttackPatternData)
            {
                attackData.AttackPattern.playerTransform = playerTransform;
                if (attackData.AttackPattern as PheremoneBlast)
                {
                    pheremoneBlast.SetUpAbilityData(attackData);
                    
            
                    currentAttackCycle.Add(pheremoneBlast);
                }
                else if(attackData.AttackPattern as SendSoldiers)
                {
                    sendSoldiers.SetUpAbilityData(attackData);
                    currentAttackCycle.Add(sendSoldiers);

                }
                else if (attackData.AttackPattern as AttackDrones)
                {
                    attackDrones.SetUpAbilityData(attackData);
                    currentAttackCycle.Add(sendSoldiers);

                }
            }

            if (stageData.MaxCycleTime > 0)
            {
                foreach (BaseAttackPattern attackPattern in currentAttackCycle)
                {
                    attackPattern.AttackEnded += NextAttackPattern;
                }
                StartNewCycleStage();
            }
            else
            {
                StartNewStage();
            }


        }
    }
    private void StartNewCycleStage()
    {
        //currCycleIndex = 0;
        //currentAttackCycle[currCycleIndex].BeginAttackPattern();
    }
    private void StartNewStage()
    {
        //foreach (BaseAttackPattern attackPattern in currentAttackCycle)
        //{
        //    attackPattern.BeginAttackPattern();
        //}
    
    }
    private BossStageData GetStageData(BossStage stage)
    {
        for (int i = 0; i < stageDatas.Count; i++)
        {
            if (stageDatas[i].Stage == stage) return stageDatas[i];
        }
        return null;
    }
    private void EvaluateNewState(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.MainPowerOff:
                StartBossBattle();


                break;

        }
    }

    private void OnDestroy()
    {
        GameStateManager.instance.OnGameStateChange -= EvaluateNewState;
    }
    private void OnDisable()
    {
        GameStateManager.instance.OnGameStateChange -= EvaluateNewState;
    }
}
