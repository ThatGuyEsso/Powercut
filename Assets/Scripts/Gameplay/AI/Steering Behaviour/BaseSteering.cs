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

    virtual public void Init(float maxSpeed)
    {
  
        rb = GetComponent<Rigidbody2D>();
        this.maxSpeed = maxSpeed;

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


}
