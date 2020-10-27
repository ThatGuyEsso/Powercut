using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
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
    public void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();

        gunsCarried[0] = GunTypes.Pistol;
        gunsCarried[1] = GunTypes.Shotgun;
        equippedGun = GunTypes.Shotgun;
    }
    private void Start()
    {
        WeaponManager.instance.SetActiveWeapon(GunTypes.Shotgun);
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
    }

    public void FixedUpdate()
    {
        //Movement loopUpdate
        if (isMoving)
        {
            rb.velocity = new Vector2(xInput, yInput).normalized * settings.maxSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
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


}
