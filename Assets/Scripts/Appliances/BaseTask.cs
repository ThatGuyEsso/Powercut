using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaseTask : MonoBehaviour, Controls.IInteractionsActions, IBreakable
{
    [Header("Settings")]
    public float maxHealth;
    protected float currHealth;
    public float fixTickRate;
    public float fixAmountPerTick;
    private float currFixTime;
    public string taskName;//Task Settings
    public string taskDescription;

    public Sprite[] stateSprites;//0 should be fixed max should be max broken
    public GameObject fixVFX;

    //input
    protected Controls input;

    //Component
    private SpriteRenderer gfx;

    //object states
    protected bool inRange;
    protected bool isFixed;
    protected bool isFixing;


    public void Init()
    {
        //Inputs
        input = new Controls();
        input.Interactions.SetCallbacks(this);
        input.Enable();
        //Set initial variables
        currHealth = 0f;
        isFixed = false;
        gfx = gameObject.GetComponentInChildren<SpriteRenderer>();
        UpdateDamageDisplay();
    }

    protected void FixedUpdate()
    {
        if (isFixing && !isFixed)
        {
            if (!GetIsFixed())
            {
        
                if (currFixTime <= 0)
                {
                    currHealth += fixAmountPerTick;
                    currFixTime = fixTickRate;
                    if(currFixTime>= maxHealth)
                    {
                        
                    }
                }
                else
                {
                    currFixTime -= Time.deltaTime;
                }
            }
            else{
                UIManager.instance.eventDisplay.CreateEvent(taskDescription + " Fixed", Color.green);
            }
           
        }
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isFixed)
            {
                inRange = true;
                InGamePrompt.instance.ChangePrompt("[E] To Fix Motor");
                InGamePrompt.instance.ShowPrompt();

            }

        }

    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRange = false;
            isFixing = false;
            InGamePrompt.instance.HidePrompt();
        }


    }
    //---------------------------------------------------------
    //Input actions
    //---------------------------------------------------------
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && inRange)
        {
            //Get current state
            switch (GameStateManager.instance.GetCurrentGameState())
            {
                //If Main power is of player can begin to fix 
                case GameStates.MainPowerOff:
                    //Begin fixing
                    isFixing = true;
                    Debug.Log("Should begin fixing");

                    break;

                default:
                    Debug.Log("Can't fix main power is still on");
                    break;
            }
        }
    }
    //---------------------------------------------------------
    //Interfaces
    //---------------------------------------------------------
    void IBreakable.Damage(float damage, BaseEnemy interfacingEnemy)
    {

        DamageMotor(damage);
        if (!isFixed)
        {
            interfacingEnemy.GetComponent<IBreakable>().ObjectIsBroken();
        }
    }

    void IBreakable.ObjectIsBroken()
    {

    }

    protected void DamageMotor(float damage)
    {
        //Check if lamp is working before damaging lamp
        if (isFixed)
        {
            currHealth -= damage;//firstly subtact damage from currentHealth

            //Check wether the lamp is still has health left if not it must not be working
            if (currHealth <= 0)
            {


                isFixed = false;
                currHealth = 0;
                TaskManager.instance.RecordFailedTask(taskName);
                UIManager.instance.eventDisplay.CreateEvent(taskDescription + " broken",Color.red);
            }

        }
        UpdateDamageDisplay();
    }

    protected void UpdateDamageDisplay()
    {
        if (isFixed && currHealth >= maxHealth)
        {
            gfx.sprite = stateSprites[0];
        }
        else
        {
            EvaluateSpriteDisplay();
        }
        
    }

    protected void EvaluateSpriteDisplay()
    {
        float percentageHealth = currHealth / maxHealth;

        if(percentageHealth <= 0.25f)
        {
            gfx.sprite = stateSprites[stateSprites.Length - 1];
        }else if(percentageHealth > 0.25f && percentageHealth <= 0.5f)
        {
            gfx.sprite = stateSprites[stateSprites.Length - 2];
        }
        else if(percentageHealth > 0.5f && percentageHealth <= 0.75f)
        {
            gfx.sprite = stateSprites[stateSprites.Length - 3];
        }
    }

    public bool GetIsFixed()
    {
        if (currHealth >= maxHealth)
        {
            isFixed = true;
            TaskManager.instance.RecordCompletedTask(taskName);
        }
        else
        {
            isFixed = false;
        }
        UpdateDamageDisplay();
        return isFixed;
    }

    public string GetTaskName()
    {
        return taskName;
    }
    public void ResetTask()
    {
        currHealth = 0f;
        isFixed = false;
        UpdateDamageDisplay();
    }
    void OnDestroy()
    {
        input.Disable();
    }
}
