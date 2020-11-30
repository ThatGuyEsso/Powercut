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
public class PlayerBehaviour : MonoBehaviour,IHurtable, Controls.IPlayerControlsActions,IPlayerComponents
{
   

    //Settings
    public PlayerSettings settings;
    private float smoothRot;
    private PlayerAnimController animControl;
    private Vector2 knockBack;
    public float knockBackFallOff = 0.1f;
    [SerializeField]
    private string hurtSFX;
    [SerializeField]
    private string stepSFX;
    //Object Components
    public Camera activeCamera;
    public FieldOfView fieldOfView;
    private Rigidbody2D rb;
    public Transform throwingPoint;
    //Weapons
    private GunTypes[] gunsCarried = new GunTypes[2];
    private GunTypes equippedGun;
    private GadgetTypes[] gadgetCarried = new GadgetTypes[1];
    private int numberOfPrimaryGadget;
    private int numberOfSecondaryGadget;

    //Timers
    private float currHealth;
    private float currHurtTime;
    [SerializeField]
    private float maxTimeBtwnSteps;
    private float currTimeBetwenSteps;
    //States
    private bool canBeHurt;
    private MovementStates currMoveState;
    private bool isDead;
    private bool isInitialised;

    //Input
    private Vector2 moveDir;
    private Controls input;
    private int equippedIndex;
    public void Init()
    {
        //Cache
        rb = gameObject.GetComponent<Rigidbody2D>();
        animControl = gameObject.GetComponent<PlayerAnimController>();

        //Set initial variables
        gunsCarried[0] = GunTypes.Pistol;
        gunsCarried[1] = GunTypes.Shotgun;
        gadgetCarried[0] = GadgetTypes.FlashBang;
        currHealth = settings.maxHealth;
        currHurtTime = settings.maxHurtTime;
        numberOfPrimaryGadget = 3;
        numberOfSecondaryGadget = 2;

        CycleBetweenGuns();
        SetUpHealth();
        animControl.UpdatePlayergun();
        WeaponManager.instance.SetUpGadget(gadgetCarried, numberOfPrimaryGadget, numberOfSecondaryGadget);
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
            if (!isDead)
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
                    rb.velocity = knockBack;
                    //knockback eventually falls off
                    knockBack = Vector2.Lerp(knockBack, Vector2.zero, knockBackFallOff);
                    if (knockBack.magnitude < 0.5f)
                    {
                        knockBack = Vector2.zero;
                    }
                }
                else{
                    rb.velocity = Vector2.zero;

                }
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
        //If it is performed character should move
        if (context.performed)
        {
            moveDir = context.ReadValue<Vector2>(); // gets direction of movement
            SetMovementState(MovementStates.Walking);
            animControl.PlayWalkAnim();
            //currTimeBetwenSteps = maxTimeBtwnSteps;
        }
    }

    //on shoot input action
    public void OnShoot(InputAction.CallbackContext context)
    {
        if (GameStateManager.instance.GetCurrentGameState() != GameStates.MainPowerOn)
        {

            if (context.performed)
            {
                WeaponManager.instance.ShootActiveWeapon();//Trigger Weapon manager to reload
            }
        }
    }
    //on reload input action
    public void OnReload(InputAction.CallbackContext context)
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

    //on Switch Weapon input action
    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        animControl.UpdatePlayergun();
        //Needs to be refactured 
        if (context.performed)
        {
            
            CycleBetweenGuns();
        }
    }

    public void OnUsePrimaryGadget(InputAction.CallbackContext context)
    {
        //Needs to be refactured 
        if (context.performed)
        {
            if (numberOfPrimaryGadget > 0)
            {
                numberOfPrimaryGadget--;
                WeaponManager.instance.UsePrimaryGadget(numberOfPrimaryGadget,transform.up,throwingPoint.position);
                
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
        if (canBeHurt)
        {
            AudioManager.instance.PlayRandFromGroup("CharacterHurtSFX");
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
        if (isInitialised)
        {
             rb.velocity =  (moveDir.normalized * settings.maxSpeed * Time.deltaTime)+knockBack;
            PlayStepSFX();
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

    private void PlayerDie()
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
        
    }

    private void OnDestroy()
    {
        if (input != null)
        {
            input.Disable();
            input.PlayerControls.Movement.canceled -= _ => EndMovement();
        }
    }

    private void CycleBetweenGuns()
    {
        //Increment index
        equippedIndex++;
        //If index reaches end of array
        if (equippedIndex >= gunsCarried.Length)
        {
            equippedIndex = 0;//loop back round
        }

        //equipped gun is the respective gun
        equippedGun = gunsCarried[equippedIndex];
        
        //Update weapon manager
        WeaponManager.instance.SetActiveWeapon(equippedGun);
    }

  
    public void PlayStepSFX()
    {
        if (currTimeBetwenSteps <= 0f)
        {
            AudioManager.instance.PlayRandFromGroup("CharacterStepsSFX");
            currTimeBetwenSteps = maxTimeBtwnSteps;
        }
        else
        {
            currTimeBetwenSteps -= Time.deltaTime;
        }
    }
}
