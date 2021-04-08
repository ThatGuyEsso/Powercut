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

    [Header("Settings")]
    [SerializeField] private float MaxHealth;
    private float currHealth;
    [SerializeField] private float hurtTime;


    [Header("Boss Components")]
    [SerializeField] private List<BroodNestDelegate> broodDelegates = new List<BroodNestDelegate>();
    [SerializeField] private ScalingProgressBar healthBar;
    [SerializeField] private BossHealthAnimController healthBarAnim;
    [SerializeField] private HurtFlash hurtVFX;
    public void Damage(float damage, Vector3 knockBackDir, float knockBack)
    {
        if(currentStage!= BossStage.Transition)
        {
            if (!isHurt)
            {
                hurtVFX.BeginFlash();
                isHurt = true;
                currHealth -= damage;
                if (damage < 0f) damage = 0f;
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
    }
    public void StartBossBattle()
    {
        GameStateManager.instance.BeginNewGameState(GameStates.MainPowerOff);
      
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
   
}
