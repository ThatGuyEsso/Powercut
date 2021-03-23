using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive : BaseSteering
{
    [SerializeField] protected float slowingRadius;
    virtual public Vector2 CalculateArrivalForce()
    {
        if (!target) return Vector2.zero;
        Vector2 desiredVel = ((Vector2)target.position - rb.position);
        float distance = Vector2.Distance((Vector2)target.position, rb.position);

        if(distance < slowingRadius)
        {
            desiredVel = desiredVel.normalized * maxSpeed *Time.deltaTime*(distance / slowingRadius);

        }
        else
        {
            desiredVel = desiredVel.normalized * maxSpeed * Time.deltaTime;
        }
        resultantForce = desiredVel - rb.velocity;
        return resultantForce;
    }

    override public Vector2 CalculateResultantForce()
    {
        return CalculateArrivalForce();
    }

    public void SetSlowingRadius(float radius) { slowingRadius = radius; }
}
