using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MovementStates
{
    Walking,
    Idle
};

public class PlayerBehaviour : MonoBehaviour,IHurtable, Controls.IPlayerControlsActions
{
   

    //Settings
    public PlayerSettings settings;
    private float smoothRot;
    private Vector2 knockBack;
    public float knockBackFallOff = 0.1f;
    //Object Components
    public Camera activeCamera;
    public FieldOfView fieldOfView;
    private Rigidbody2D rb;

    //Weapons
    private GunTypes[] gunsCarried = new GunTypes[2];
    private GunTypes equippedGun;

    //Timers
    private float currHealth;
    private float currHurtTime;

    //States
    private bool canBeHurt;
    private MovementStates currMoveState;

    //Input
    private Vector2 moveDir;
    private Controls input;
    public void Awake()
    {
        //Cache
        rb = gameObject.GetComponent<Rigidbody2D>();

        //Set initial variables
        gunsCarried[0] = GunTypes.Pistol;
        gunsCarried[1] = GunTypes.Shotgun;
        equippedGun = GunTypes.Shotgun;
        currHealth = settings.maxHealth;
        currHurtTime = settings.maxHurtTime;

        //Set input
        input = new Controls();
        input.PlayerControls.SetCallbacks(this);
        input.Enable();

        input.PlayerControls.Movement.canceled += _ => EndMovement();
    }
    private void Start()
    {
        WeaponManager.instance.SetActiveWeapon(GunTypes.Shotgun);
        SetUpHealth();
    }
    public void Update()
    {



        fieldOfView.SetAimDirection(transform.up);
        fieldOfView.SetOrigin(transform.position);
   
        //Update player rotation
        PlayerFacePointer();

        //isHurt
        if (!canBeHurt)
        {
            if(currHurtTime <= 0)
            {
                canBeHurt =true;
                currHurtTime = settings.maxHurtTime;
            }
            else
            {
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
                if (knockBack.magnitude>0)
                {
                    rb.velocity = knockBack;
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

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            moveDir = context.ReadValue<Vector2>();
            SetMovementState(MovementStates.Walking);
        }
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            WeaponManager.instance.ShootActiveWeapon();
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            WeaponManager.instance.ReloadActiveWeapon();
        }
    }

    public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
        
            if (equippedGun == GunTypes.Shotgun)
            {
                equippedGun = GunTypes.Pistol;
                WeaponManager.instance.SetActiveWeapon(equippedGun);
            }
            else if (equippedGun == GunTypes.Pistol)
            {
                equippedGun = GunTypes.Shotgun;
                WeaponManager.instance.SetActiveWeapon(equippedGun);

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


    public void SetUpHealth()
    {
        UIManager.instance.healthBarDisplay.InitSlider(settings.maxHealth);
        UIManager.instance.healthBarDisplay.UpdateSlider(currHealth);
    }

    void IHurtable.Damage(float damage, Vector3 knockBackDir, float knockBack)
    {
        HurtPlayer(damage, knockBackDir, knockBack);
    }
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

    public void HurtPlayer(float damage,Vector2 knockBackDir,float knockBack)
    {
        if (canBeHurt)
        {
            canBeHurt = false;
            currHealth -= damage;
            if (currHealth > 0)
            {
                Debug.Log("launch player");

                this.knockBack = knockBackDir* knockBack;
            }

            UIManager.instance.healthBarDisplay.UpdateSlider(currHealth);
        }
    }
    public void Walk()
    {
        rb.velocity =  (moveDir.normalized * settings.maxSpeed * Time.deltaTime)+knockBack;
    }

    public void Stop()
    {
        rb.velocity = (moveDir.normalized * settings.maxSpeed * Time.deltaTime)+ knockBack;
    }

    public void SetMovementState(MovementStates newState)
    {
        currMoveState = newState;
    }

    private void EndMovement()
    {
        SetMovementState(MovementStates.Idle);
        rb.velocity = Vector2.zero;
    }

    private void OnDestroy()
    {
        input.Disable();

    }
}
