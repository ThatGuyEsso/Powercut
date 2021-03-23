using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : BaseSteering
{
    virtual public Vector2 CalculateSeekVelocity()
    {
        if (!target) return Vector2.zero;
        Vector2 desiredVel = ((Vector2)target.position - rb.position).normalized * Time.deltaTime * maxSpeed;
        resultantForce = desiredVel-rb.velocity;
        return resultantForce;
    }
    override public Vector2 CalculateResultantForce()
    {
        return CalculateSeekVelocity();
    }
    //protected override void Update()
    //{
    //    base.Update();
    //    CalculateSeekVelocity();
    //}
}
