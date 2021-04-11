using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//PLayer movement state
public enum MovementStates
{
    Walking,
    Idle
};

//Manages player behaviour (walking, input, health)
public class PlayerBehaviour : MonoBehaviour,IHurtable, Controls.IPlayerControlsActions,IPlayerComponents, IFixable,IPickUp
{
   

    //Settings
    public PlayerSettings settings;
    private float smoothRot;
    private PlayerAnimController animControl;
    private Vector2 knockBack;
    public float knockBackFallOff = 0.1f;
    [SerializeField]
    private string hurtSFX;
  
    //Object Components
    public Camera activeCamera;
    public FieldOfView fieldOfView;
    private Rigidbody2D rb;
    public Transform throwingPoint;
    //Weapons

    private GadgetTypes[] gadgetCarried = new GadgetTypes[1];
    private int numberOfPrimaryGadget;
    private int numberOfSecondaryGadget;
    private bool canAttack;
    [SerializeField] private GameObject audioPlayerPrefab;
    //Timers
    private float currHealth;
    private float currHurtTime;

    private float maxTimeBtwnSteps;
    private float currTimeBetwenSteps;
    //States
    private bool canBeHurt;
    private MovementStates currMoveState;
    private bool isDead;
    private bool isInitialised;
    private bool isFixing =false;

    //Input
    private Vector2 moveDir;
    private Controls input;
    private int equippedIndex;
    private float smoothAX;
    private float smoothAY;
    private float smoothDX;
    private float smoothDY;

    public void Init()
    {
        //Cache
        rb = gameObject.GetComponent<Rigidbody2D>();
        animControl = gameObject.GetComponent<PlayerAnimController>();

        //Set initial variables
        gadgetCarried[0] = GadgetTypes.FlashBang;
        currHealth = settings.maxHealth;
        currHurtTime = settings.maxHurtTime;
        numberOfPrimaryGadget = 3;
        numberOfSecondaryGadget = 2;

        CheckLoadOut();
        CycleBetweenGuns();
        SetUpHealth();
        animControl.UpdatePlayergun();

        currMoveState = MovementStates.Idle;
        //Set input
        input = new Controls();
        input.PlayerControls.SetCallbacks(this);
        input.Enable();

        input.PlayerControls.Movement.canceled += _ => EndMovement();
        isInitialised = true;
    }

    public void Update()
    {

        if (isInitialised)
        {

            //Updates direction and origin of field of view
            fieldOfView.SetAimDirection(transform.up);
            fieldOfView.SetOrigin(transform.position);

            //Update player rotation
            if (!isDead&& InitStateManager.currGameMode == GameModes.Powercut)
            {
                PlayerFacePointer();

            }
        }

        //If can't player can be hurt they must have recently be damaged
        if (!canBeHurt)
        {
            //Timer until player can be hurt again
            if(currHurtTime <= 0)
            {
                //Player can be hurt reset timer
                canBeHurt =true;
                currHurtTime = settings.maxHurtTime;
            }
            else
            {
                //Decrease timer
                currHurtTime -= Time.deltaTime;
            }
        }
    }

    
    public void FixedUpdate()
    {
        //Movement loopUpdate

        switch (currMoveState)
        {
            case MovementStates.Idle:

                //If idle keep players speed 0, unless knock back is applied
                if (knockBack.magnitude>0)
                {
                
                    //knockback eventually falls off
                    knockBack = Vector2.Lerp(knockBack, Vector2.zero, knockBackFallOff);
                    if (knockBack.magnitude < 0.5f)
                    {
                        knockBack = Vector2.zero;
                    }
                }
              
                SmoothDecelerate(0.0f);

                break;

            case MovementStates.Walking:
              
                    //If moving move player in movement direction unless knock back is applied
                    if (knockBack.magnitude > 0)
                    {
                        //rb.velocity = knockBack;
                        knockBack = Vector2.Lerp(knockBack, Vector2.zero, knockBackFallOff);
                        if (knockBack.magnitude < 0.5f)
                        {
                            knockBack = Vector2.zero;
                        }
                    }
                    Walk();
                
              
               
          
                break;

        }

    }

    //Get input action to move
    public void OnMovement(InputAction.CallbackContext context)
    {
        if (InitStateManager.currGameMode == GameModes.Powercut)
        {
            //If it is performed character should move
            if (context.performed)
            {
                moveDir = context.ReadValue<Vector2>(); // gets direction of movement
                SetMovementState(MovementStates.Walking);
                animControl.PlayWalkAnim();
                //currTimeBetwenSteps = maxTimeBtwnSteps;
            }
        }
    }

    //on shoot input action
    public void OnShoot(InputAction.CallbackContext context)
    {
      
        if (GameStateManager.instance.GetCurrentGameState() != GameStates.MainPowerOn && InitStateManager.currGameMode == GameModes.Powercut)
        {
            if (canAttack)
            {
                if (context.performed)
                {
                    WeaponManager.instance.ShootActiveWeapon();//Trigger Weapon manager to reload
                }
            }
        }
        
    }
    //on reload input action
    public void OnReload(InputAction.CallbackContext context)
    {
        if (InitStateManager.currGameMode == GameModes.Powercut)
        {
            if (canAttack)
            {
                if (GameStateManager.instance.GetCurrentGameState() != GameStates.MainPowerOn)
                {
                    if (context.performed)
                    {
                        //tell weapon manager to reload active weapon
                        WeaponManager.instance.ReloadActiveWeapon();
                    }
                }
            }
        }
    }

    //on Switch Weapon input action
    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if(InitStateManager.currGameMode == GameModes.Powercut)
        {
        
            animControl.UpdatePlayergun();
            //Needs to be refactured 
            if (context.performed)
            {

                CycleBetweenGuns();
         
            }
            
        }

    }

    public void OnUsePrimaryGadget(InputAction.CallbackContext context)
    {

        if (InitStateManager.currGameMode == GameModes.Powercut)
        {
            if (canAttack)
            {
                //Needs to be refactured 
                if (context.performed)
                {
                    if (numberOfPrimaryGadget > 0)
                    {
                        numberOfPrimaryGadget--;
                        WeaponManager.instance.UsePrimaryGadget(numberOfPrimaryGadget, transform.up, throwingPoint.position);
                        AudioPlayer aPlayer = ObjectPoolManager.Spawn(audioPlayerPrefab, transform.position, Quaternion.identity)
                            .GetComponent<AudioPlayer>();
                        aPlayer.SetUpAudioSource(AudioManager.instance.GetSound("Throw"));
                        aPlayer.Play();


                    }

                }
            }

        }
    }
    public void PlayerFacePointer()
    {
        float targetAngle = Mathf.Atan2(EssoUtility.GetVectorToPointer(activeCamera,transform.position).y, EssoUtility.GetVectorToPointer(activeCamera, transform.position).x) * Mathf.Rad2Deg;//get angle to rotate
        targetAngle -= 90f;// turn offset -Due to converting between forward vector and up vector
        //if (targetAngle < 0) targetAngle += 360f;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle, ref smoothRot, settings.rotationSpeed);//rotate player smoothly to target angle
        transform.rotation = Quaternion.Euler(0f, 0f, angle);//update angle
        //fovObject.SetAimDirection((-1)*fovObject.GetVectorFromAngle(angle));
    }


    //Initialises Health and health UI
    public void SetUpHealth()
    {
        UIManager.instance.healthBarDisplay.InitSlider(settings.maxHealth);
        UIManager.instance.healthBarDisplay.UpdateSlider(currHealth);
    }

    //Interfaces when player receives damage
    void IHurtable.Damage(float damage, Vector3 knockBackDir, float knockBack)
    {
        HurtPlayer(damage, knockBackDir, knockBack);
        
        
    }

    void IPlayerComponents.PlayerDied()
    {
        //Only needs to be able to interface with components so  left blank
    }
    void IPlayerComponents.PlayerReset()
    {
        //Only needs to be able to interface with components so  left blank
    }
    //Calculates if player has health left if not return false, Hence player is dead
    public bool GetIsAlive()
    {
        if (currHealth > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
        
    }


    //Updates UI, apply damage and damage force to player
    public void HurtPlayer(float damage,Vector2 knockBackDir,float knockBack)
    {
        //Only hurt player if player can be hurt
        if (canBeHurt && currHealth >0)
        {
            AudioManager.instance.PlayRandFromGroup(hurtSFX);
            CamShake.instance.DoScreenShake(settings.duration, settings.magnitude, settings.smoothIn, settings.smoothOut);

            canBeHurt = false;//just been hurt so shouldn't hurt player again until timer has finished
            currHealth -= damage;
            Debug.Log("launch player");
            this.knockBack = knockBackDir* knockBack;

            if (!GetIsAlive())
            {
                PlayerDie();
            }

            UIManager.instance.healthBarDisplay.UpdateSlider(currHealth);
        }
    }
    public void Walk()
    {
        if (isInitialised && InitStateManager.currGameMode == GameModes.Powercut)
        {
            Vector2 targetVel = Vector2.zero;

            targetVel.x = Mathf.SmoothDamp(rb.velocity.x, settings.maxSpeed * moveDir.normalized.x , ref smoothAX, settings.timeZeroToMax);
            targetVel.y = Mathf.SmoothDamp(rb.velocity.y, settings.maxSpeed * moveDir.normalized.y , ref smoothAY, settings.timeZeroToMax);
            rb.velocity = targetVel * Time.deltaTime  /*(moveDir.normalized * settings.maxSpeed * Time.deltaTime)*/+ knockBack;

        }
        else
        {
            SetMovementState(MovementStates.Idle);
        }
    }

    virtual protected void SmoothDecelerate(float minSpeed)
    {
        Vector2 targetVelocity = Vector2.zero;
        if (rb)
        {
            if (rb.velocity.magnitude <= 0.1f)
            {
                smoothAX = 0f;
                smoothAY = 0f;
                smoothDX = 0f;
                smoothDY = 0f;
            }
            else
            {
                targetVelocity.x = Mathf.SmoothDamp(rb.velocity.x, minSpeed, ref smoothDX, settings.timeMaxToZero);
                targetVelocity.y = Mathf.SmoothDamp(rb.velocity.y, minSpeed, ref smoothDY, settings.timeMaxToZero);
            }

            rb.velocity = targetVelocity * Time.deltaTime + knockBack;
        }
     
    }
    public void SetMovementState(MovementStates newState)
    {
        currMoveState = newState;
    }

    private void EndMovement()
    {
        SetMovementState(MovementStates.Idle);
        rb.velocity = Vector2.zero;
        animControl.StopWalkAnim();
    }

    public void PlayerDie()
    {
        input.Disable();
        SetMovementState(MovementStates.Idle);
        input.PlayerControls.Movement.canceled -= _ => EndMovement();
        isDead = true;

        IPlayerComponents[] components = gameObject.GetComponentsInChildren<IPlayerComponents>();
        foreach(IPlayerComponents component in components)
        {
            component.PlayerDied();
        }
        fieldOfView.ToggleLight(false);
        InitStateManager.instance.BeginNewState(InitStates.PlayerDead);
        
    }

    private void OnDestroy()
    {
        if (input != null)
        {
            input.Disable();
            input.PlayerControls.Movement.canceled -= _ => EndMovement();
        }

        InitStateManager.instance.OnStateChange -= EvaluateNewState;
        GameStateManager.instance.OnGameStateChange -= EvaluateGameNewState;
    }

    private void CycleBetweenGuns()
    {
        //Update weapon manager
        WeaponManager.instance.SwitchWeapons();
    }

  


    private void ResetCharacter()
    {
        currHealth = settings.maxHealth;
        currHurtTime = settings.maxHurtTime;
        numberOfPrimaryGadget = 3;
        WeaponManager.instance.SetUpGadget(gadgetCarried, numberOfPrimaryGadget, numberOfSecondaryGadget);
        numberOfSecondaryGadget = 2;
        NotFixing();
        IPlayerComponents[] components = gameObject.GetComponentsInChildren<IPlayerComponents>();
        foreach (IPlayerComponents component in components)
        {
            component.PlayerReset();
        }
        isDead = false;
        SetUpHealth();
        animControl.UpdatePlayergun();
        input.Enable();
    }


    public void BindToInitManager()
    {
        InitStateManager.instance.OnStateChange += EvaluateNewState;
        GameStateManager.instance.OnGameStateChange += EvaluateGameNewState;
    }
    private void EvaluateNewState(InitStates newState)
    {
        switch (newState)
        {
            case InitStates.PlayerSpawned:
                Init();
                WeaponManager.instance.Init();

                break;
            case InitStates.PlayerRespawned:
                ResetCharacter();

     
                break;
        }
    }


    private void EvaluateGameNewState(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.LevelClear:
                animControl.UpdatePlayergun();
                canAttack = false;
                break;
            case GameStates.MainPowerOff:
                animControl.UpdatePlayergun();
                canAttack = true;
                break;
        }
    }

    private void CheckLoadOut()
    {
        //get all gun components
        BaseGun[] guns = gameObject.GetComponentsInChildren<BaseGun>();
        //pass  values to weapon manager
        WeaponManager.instance.InitEquippedGuns(guns);

        //Pass in all gadget data too (should be separate gadget manger)
        WeaponManager.instance.SetUpGadget(gadgetCarried, numberOfPrimaryGadget, numberOfSecondaryGadget);
    }


    public void DisableControl()
    {
        input.Disable();
    }
    public void EnableControls()
    {
        input.Enable();
    }

    public bool CanFix()
    {
        if(isFixing == true)
        {
            return false;
        }
        else
        {
            isFixing = true;
            return true;
        }
    }

    public void NotFixing()
    {
        isFixing = false;
    }

    public void PickUpHealth(float health)
    {
        currHealth += health;
        if (currHealth > settings.maxHealth) currHealth = settings.maxHealth;
        UIManager.instance.healthBarDisplay.UpdateSlider(currHealth);
        AudioManager.instance.PlayIfFree("Relief");
    }

    public void PickUpAmmo()
    {
        WeaponManager.instance.RefreshWeaponAmmo();
        
    }

    public void Push(Vector3 knockBackDir, float knockBack)
    {
        rb.AddForce(knockBackDir * knockBack, ForceMode2D.Impulse);
    }
}
