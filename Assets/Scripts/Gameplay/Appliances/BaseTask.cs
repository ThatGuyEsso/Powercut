using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(AudioSource))]
public abstract class BaseTask : MonoBehaviour, Controls.IInteractionsActions, IBreakable,IFixable
{
    [Header("Settings")]
    public float maxHealth;
    protected float currHealth;
    public float fixTickRate;
    public float fixAmountPerTick;
    private float currFixTime;
    public string taskName;//Task Settings
    public string taskDescription;



    [SerializeField] private SpriteRenderer icon;
    [SerializeField] private Sprite BrokenSprite;
    [SerializeField] private Sprite FixedSprite;
    [SerializeField] protected string playerPrompt;
    [SerializeField] protected Color inRangePromptColour = Color.white;
    [SerializeField] protected string powerStillOnPrompt;
    [SerializeField] protected string fixingPrompt;
    public Sprite[] stateSprites;//0 should be fixed max should be max broken
    public GameObject fixVFX;
    protected GameObject player;
    //input
    protected Controls input;

    //Component
    [SerializeField] protected SpriteRenderer gfx;

    //object states
    protected bool inRange;
    protected bool isFixed;
    protected bool isFixing;
    protected bool isRecorded =false;
    protected AudioSource audioSource;

    [SerializeField] protected Color fixedColor;
    [SerializeField] protected Color fixingColor;
    [SerializeField] protected ChargingCable fixingCable;
    protected Transform playerTransform;
    protected bool isInitialised;
    public void Init()
    {
        //Inputs
        input = new Controls();
        input.Interactions.SetCallbacks(this);
        input.Enable();
        isInitialised = true;
        //Set initial variables
        currHealth = 0f;
        isFixed = false;
        inRange = false;
      
        UpdateDamageDisplay();
        icon.transform.rotation = Quaternion.Euler(Vector2.up);
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource)
        {
            Sound sound = AudioManager.instance.GetSound("DeployCableSFX");
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
        }
    }

    virtual protected void Update()
    {
        if (isFixing && !isFixed)
        {
            if (!GetIsFixed())
            {
        
                if (currFixTime <= 0)
                {
                    currHealth += fixAmountPerTick;
                    currFixTime = fixTickRate;
                    if(currHealth>= maxHealth)
                    {
                        currHealth = maxHealth;
                    }
                }
                else
                {
                    currFixTime -= Time.deltaTime;
                }
            }
            else{
                if(player!=false)
                    player.GetComponent<IFixable>().NotFixing();
                UIManager.instance.eventDisplay.CreateEvent(taskDescription + " Fixed", Color.green);
            }
           
        }
    }

    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && !isFixed)
        {
         
                inRange = true;
                InGamePrompt.instance.SetColor(inRangePromptColour);
                InGamePrompt.instance.ChangePrompt(playerPrompt);
                InGamePrompt.instance.ShowPrompt();
                player = other.gameObject;
                fixingCable.ChangeColour(fixingColor);
                playerTransform = other.transform;
                player = other.gameObject;
            

        }

    }

    virtual protected void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && fixingCable.isDrawing) fixingCable.StopDrawingRope();
        {
       
            inRange = false;
            isFixing = false;
            playerTransform = null;
            InGamePrompt.instance.HidePrompt();

            if (player != false)
            {
                player.GetComponent<IFixable>().NotFixing();
                player = null;
            }
        }

    
    }
    //---------------------------------------------------------
    //Input actions
    //---------------------------------------------------------
    virtual public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed && inRange)
        {
            switch (GameStateManager.instance.GetCurrentGameState())
            {

                //If Main power is of player can begin to fix 
                case GameStates.MainPowerOff:
                    //Begin fixing
                    if (!isFixed)
                    {
                        if (player != false)
                        {
                            if (player.GetComponent<IFixable>().CanFix())
                            {

                                isFixing = true;
                                Debug.Log("Should begin fixing");
                                InGamePrompt.instance.SetColor(Color.green);
                                InGamePrompt.instance.ShowPromptTimer(fixingPrompt, 5.0f);

                                if (playerTransform != false) fixingCable.StartDrawingRope(playerTransform);
                                audioSource.Play();
                            }
                        }
                    }

                    break;

                default:
                    InGamePrompt.instance.SetColor(Color.red);
                    InGamePrompt.instance.ShowPromptTimer(powerStillOnPrompt, 5.0f);

                    break;
            }
        }
    }

    //---------------------------------------------------------
    //Interfaces
    //---------------------------------------------------------
    void IBreakable.Damage(float damage, BaseEnemy interfacingEnemy)
    {

        DamageTask(damage);
        if (!isFixed)
        {
            interfacingEnemy.GetComponent<IBreakable>().ObjectIsBroken();
        }
    }

    void IBreakable.ObjectIsBroken()
    {

    }

    protected void DamageTask(float damage)
    {
        //Check if lamp is working before damaging lamp
        if (isFixed)
        {
            currHealth -= damage;//firstly subtact damage from currentHealth

            //Check wether the lamp is still has health left if not it must not be working
            if (currHealth <= 0)
            {
                icon.sprite = BrokenSprite;
                isRecorded = false;
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
        if (isFixed )
        {
            gfx.sprite = stateSprites[0];
      
        }
        else
        {
            EvaluateSpriteDisplay();
        }
        
    }

    virtual protected void EvaluateSpriteDisplay()
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
            icon.sprite = FixedSprite;
                  if (fixingCable.isDrawing)
            {
                fixingCable.ChangeColour(fixedColor);
            }
            if (isRecorded == false)
            {

                TaskManager.instance.RecordCompletedTask(taskName);
                isRecorded = true;
            }
      
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
        isRecorded = false;
        icon.sprite = BrokenSprite;
        UpdateDamageDisplay();
    }
    void OnDestroy()
    {
        if(isInitialised)
            input.Disable();
    }

    public bool CanFix()
    {
        throw new System.NotImplementedException();
    }

    public void NotFixing()
    {
        throw new System.NotImplementedException();
    }
}
