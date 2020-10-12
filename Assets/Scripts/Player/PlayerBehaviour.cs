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
    public DynamicLight lightSource;
    //public Vector3 lightOffset;
    //public float lightRoationOffset;
    public Camera activeCamera;
    public void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
     
    }
    public void Update()
    {

        lightSource.SetAimDirection(transform.up);
        lightSource.SetOrigin(transform.position);
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
        float targetAngle = Mathf.Atan2( GetVectorToPointer().y, GetVectorToPointer().x) * Mathf.Rad2Deg;//get angle to rotate
        targetAngle -= 90f;// turn offset -Due to converting between forward vector and up vector
        //if (targetAngle < 0) targetAngle += 360f;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle, ref smoothRot, settings.rotationSpeed);//rotate player smoothly to target angle
        transform.rotation = Quaternion.Euler(0f, 0f, angle);//update angle
        //fovObject.SetAimDirection((-1)*fovObject.GetVectorFromAngle(angle));
    }

    public Vector2 GetVectorToPointer()
    {
        //Get mouse position in world space
        Vector3 pointerPos = activeCamera.ScreenToWorldPoint(Input.mousePosition);

        Vector3 playerToMouse = pointerPos - transform.position;//calculate vector direction between player and cursor

        return playerToMouse.normalized;//Return normalised direction
    }
}
