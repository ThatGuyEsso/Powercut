using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour,IHurtable
{
    public PlayerSettings settings;
    private Rigidbody2D rb;
    //public FieldOfView fovObject;
    private Vector2 moveDirection;
    private bool isMoving = false;
    private float smoothRot;
    float yInput;
    float xInput;
    public FieldOfView fieldOfView;
    //public Vector3 lightOffset;
    //public float lightRoationOffset;
    public Camera activeCamera;
    private GunTypes[] gunsCarried = new GunTypes[2];
    private GunTypes equippedGun;
    private float currHealth;
    private float currHurtTime;
    private bool canBeHurt;
    public void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        gunsCarried[0] = GunTypes.Pistol;
        gunsCarried[1] = GunTypes.Shotgun;
        equippedGun = GunTypes.Shotgun;
        currHealth = settings.maxHealth;
        currHurtTime = settings.maxHurtTime;
    }
    private void Start()
    {
        WeaponManager.instance.SetActiveWeapon(GunTypes.Shotgun);
        SetUpHealth();
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            WeaponManager.instance.ShootActiveWeapon();
           
        }
        if (Input.GetMouseButtonDown(1))
        {
            WeaponManager.instance.ReloadActiveWeapon();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if(equippedGun == GunTypes.Shotgun)
            {
                equippedGun = GunTypes.Pistol;
                WeaponManager.instance.SetActiveWeapon(equippedGun);
            }else if (equippedGun == GunTypes.Pistol)
            {
                equippedGun = GunTypes.Shotgun;
                WeaponManager.instance.SetActiveWeapon(equippedGun);

            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            HurtPlayer(10f, Vector2.up, 1000f);
        }

            fieldOfView.SetAimDirection(transform.up);
        fieldOfView.SetOrigin(transform.position);
        //fovObject.SetOrigin(transform.position);
        //Get input
        yInput = Input.GetAxisRaw("Vertical");
        xInput = Input.GetAxisRaw("Horizontal");
        if (xInput != 0f || yInput != 0f)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        //Update player rotation
        PlayerFacePointer();

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

        rb.velocity = new Vector2(xInput, yInput).normalized * settings.maxSpeed;
 
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

    public void HurtPlayer(float damage,Vector3 knockBackDir,float knockBack)
    {
        if (canBeHurt)
        {
            canBeHurt = false;
            currHealth -= damage;
            if (currHealth > 0)
            {
                Debug.Log("launch player");
                rb.AddForce(knockBack * knockBackDir,ForceMode2D.Impulse);
            }

            UIManager.instance.healthBarDisplay.UpdateSlider(currHealth);
        }
    }


}
