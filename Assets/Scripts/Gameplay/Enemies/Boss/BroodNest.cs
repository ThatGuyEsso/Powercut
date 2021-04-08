using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private float MaxHealth;
    [SerializeField] private float percentNextStageTrigger;
    [SerializeField] private float tickRate;

  
    private BossStage currentStage;
    private float currHealth;
    private bool isHurt;
    [SerializeField] private ScalingProgressBar healthBar;

    [Header("Boss Components")]
    [SerializeField] private List<BroodNestDelegate> broodDelegates = new List<BroodNestDelegate>();

    public void Damage(float damage, Vector3 knockBackDir, float knockBack)
    {
        if(currentStage!= BossStage.Transition)
        {
            if (!isHurt)
            {
                currHealth -= damage;
                healthBar.UpdateValue(currHealth);
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
        healthBar.gameObject.SetActive(false);
    }

    public void InitBossBattle()
    {
        healthBar.gameObject.SetActive(true);
    }
    public void StartBossBattle()
    {
        GameStateManager.instance.BeginNewGameState(GameStates.MainPowerOff);
      
    }

    
    public void Push(Vector3 knockBackDir, float knockBack)
    {
        throw new System.NotImplementedException();
    }


   
}
