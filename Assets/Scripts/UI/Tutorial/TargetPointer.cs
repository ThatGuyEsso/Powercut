using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPointer : MonoBehaviour
{
    [SerializeField] protected Transform currentTarget;
    [SerializeField] protected Transform followTarget;
    [SerializeField] protected bool isActive;
    [SerializeField] protected GameObject gfx;
    protected float smoothRot;

    [SerializeField] protected float minTargetDistance;
    [SerializeField] protected float rotationSpeed;

    //initialise types, to set target to follow and/or to point at
    virtual public void Init(Transform followTarget)
    {
        this.followTarget = followTarget;
       

    }


    private void Awake()
    {
        InitStateManager.instance.OnStateChange += EvaluateInitState;
    }
    private void EvaluateInitState(InitStates newstate)
    {
        switch (newstate)
        {
            case InitStates.ExitLevel:
                if (gameObject != false) Destroy(gameObject);
                break;
            case InitStates.LoadTitleScreen:
                

                break;

        }
    }
    virtual public void Init(Transform followTarget, Transform pointTarget)
    {
        this.followTarget = followTarget;
        currentTarget = pointTarget;
    }
    public void SetCurrentTarget(Transform newTarget)
    {
        currentTarget = newTarget;
    }

    public void ActivatePointer()
    {
        //if target isn't false activate
        if (currentTarget != false)
        {
            gfx.SetActive(true);
            isActive = true;
        }
    }

    public void DisablePointer()
    {
            gfx.SetActive(false);
            isActive = false;
  
    }


    virtual protected void Update()
    {

        if(isActive && currentTarget != false)
        {
            PointToTarget();
            EvaluateDistance();
        }
    }

    virtual protected void LateUpdate()
    {
        if (isActive && followTarget != false)
        {
            transform.position = followTarget.position;
        }
    }
    
    private void EvaluateDistance()
    {
        if(Vector2.Distance(transform.position,currentTarget.position) <= minTargetDistance)
        {
            gfx.SetActive(false);
        }
        else
        {
            gfx.SetActive(true);
        }
    }
    public void PointToTarget()
    {
        //calculate vector to target
        Vector2 vectorToTarget = currentTarget.position - transform.position;
        float targetAngle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;//get angle to rotate
        targetAngle -= 90f;// turn offset -Due to converting between forward vector and up vector
        //if (targetAngle < 0) targetAngle += 360f;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle, ref smoothRot, rotationSpeed);//rotate player smoothly to target angle
        transform.rotation = Quaternion.Euler(0f, 0f, angle);//update angle
    
    }

}
