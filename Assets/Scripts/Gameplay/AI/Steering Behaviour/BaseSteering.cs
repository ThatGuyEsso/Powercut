using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSteering : MonoBehaviour
{

    protected Rigidbody2D rb;
    protected Transform target;
    protected bool isActive;
    protected float maxSpeed;
    protected Vector2 resultantForce = Vector2.zero;
    protected IBoid self;
    protected SteeringManager steeringManager;
    virtual public void Init(float maxSpeed, SteeringManager manager)
    {
  
        rb = GetComponent<Rigidbody2D>();
        self = GetComponent<IBoid>();
        this.maxSpeed = maxSpeed;
        steeringManager = manager;
    }

    virtual public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    virtual public void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }

    public bool IsActive() { return isActive; }
    abstract public Vector2 CalculateResultantForce();

    //virtual protected void Update()
    //{
    //    if (!isActive) return;
    //}

    public Vector2 GetResultantForce() { return resultantForce; }
    public Vector2 Evade(Vector2 targetPosition, Vector2 targetVelocity,float targetMaxSpeed)
    {
        //calculate where character will be in one frame
        float distance = Vector2.Distance(targetPosition, rb.position);
        float updatesAhead = distance / targetMaxSpeed*Time.deltaTime;
        
        //return future position
        Vector2 futurePos = PredictTargetPosition(targetPosition, targetVelocity, updatesAhead);

        //calculate evade force of targets future position
        Vector2 EvadeForce = (futurePos - rb.position).normalized * steeringManager.GetMaxEvadeForce()*Time.deltaTime;
        return EvadeForce;
    }

    public Vector2 PredictTargetPosition(Vector2 targetPosition,Vector2 targetVelocity, float updatesAhead)
    {
        return targetPosition + (targetVelocity * updatesAhead);
    } 

}
