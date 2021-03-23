using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLeader : Arrive
{

    [SerializeField] protected float separationRadius = 1.0f;
    [SerializeField] protected float maxSeparation = 1.5f;

    [SerializeField] protected IBoid leader;
  
    List<Transform> neighbours = new List<Transform>();
    Vector2 behindPoint;
    public Vector2 CalculateBehind()
    {
        Vector2 targetVel = leader.GeRightVector() * -1;

        targetVel = targetVel.normalized * leader.GetBehindLength();

        return leader.GetPosition() + targetVel;

    }

    //public Vector2 FollowLeaderBehind(Vector2 behindPos)
    //{

    //}

    public void SetLeader(IBoid newLeader)
    {
        leader = newLeader;
    }

    public override Vector2 CalculateResultantForce()
    {
        Vector2 netForce = Vector2.zero; ;
        if (leader != null)
        {
            if (IsOnLeaderLineOfSight(CalculateAhead()))
            {
                netForce += Evade(leader.GetPosition(), leader.GetVelocity(), leader.GetMaxSpeed());
            }

            behindPoint = CalculateBehind();
            netForce += CalculateArrivalForce();
            netForce += SeparationForce();
            return netForce;
        }
  

        return Vector2.zero;

    }

    override public Vector2 CalculateArrivalForce()
    {
        if (!target) return Vector2.zero;
        Vector2 desiredVel = (behindPoint - rb.position);
        float distance = Vector2.Distance(behindPoint, rb.position);

        if (distance < slowingRadius)
        {
            desiredVel = desiredVel.normalized * maxSpeed * Time.deltaTime * (distance / slowingRadius);

        }
        else
        {
            desiredVel = desiredVel.normalized * maxSpeed * Time.deltaTime;
        }
        resultantForce = desiredVel - rb.velocity;
        return resultantForce;
    }


    public Vector2 SeparationForce()
    {
        Vector2 force = Vector2.zero;

        for( int i=0;i< neighbours.Count; i++)
        {
            Vector2 currNeightPos = neighbours[i].position;
            if(Vector2.Distance(rb.position,currNeightPos)<= separationRadius)
            {
                force += (currNeightPos - rb.position);
            }
        }

        if (neighbours.Count > 0)
        {
            force /= neighbours.Count;
            force *= -1;
         
        }

        force= force.normalized * maxSeparation*Time.deltaTime;
        return force;
    }
  
    public Vector2 CalculateAhead()
    {

        Vector2 targetVel = leader.GeRightVector();

        targetVel = targetVel.normalized * leader.GetSightLength();

        return leader.GetPosition() + targetVel;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            neighbours.Add(other.transform);
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            neighbours.Remove(other.transform);
        }
    }

    public bool IsOnLeaderLineOfSight(Vector2 ahead)
    {
        if (Vector2.Distance(ahead, rb.position) <= leader.GetSightLength())
            return true;
        else if (Vector2.Distance(leader.GetPosition(), rb.position) <= leader.GetRadius()) 
            return true;
        else
            return false;
    }

}
