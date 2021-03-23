using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringManager : MonoBehaviour
{
    private bool useSteering = false;
    IBoid parent;
   public List<BaseSteering> steeringBehaviours = new List<BaseSteering>();
    Vector2 netForce;

    protected Rigidbody2D rb;
    public void Init(float maxSpeed,bool useSteering)
    {
        BaseSteering[] steerings = gameObject.GetComponents<BaseSteering>();

        for(int i=0; i< steerings.Length; i++)
        {
            steeringBehaviours.Add(steerings[i]);
            steerings[i].Init(maxSpeed);
        }
        rb = GetComponent<Rigidbody2D>();
        parent = gameObject.GetComponent<IBoid>();
        this.useSteering =useSteering;
    }

    public void BeginSeek()
    {
        DeactivateAll();
        foreach (Seek steering in steeringBehaviours)
        {
            if (steering)
            {
                steering.enabled = true;
                steering.SetActive(true); 
            }
        }
    }

    public void EndSeek()
    {
        foreach (Seek steering in steeringBehaviours)
        {
            if (steering)
            {
                steering.SetActive(false);
            
            }
        }
    }
    public void BeginArrival(float arrivalRadius)
    {
        DeactivateAll();
        foreach (Arrive steering in steeringBehaviours)
        {
            if (steering)
            {
              
                steering.SetActive(true);
                steering.SetSlowingRadius(arrivalRadius);
            }
        }
    }
    public void EndArrival()
    {
        foreach (Arrive steering in steeringBehaviours)
        {
            if (steering)
            {
                steering.SetActive(false);
               
            }
        }
    }

    public void BeginFollowLeader(IBoid leader, float arrivalRadius)
    {
        DeactivateAll();
        foreach (BaseSteering steering  in steeringBehaviours)
        {
            if (steering as FollowLeader)
            {
                FollowLeader newSteer = steering as FollowLeader;
                newSteer.SetActive(true);
            
                newSteer.SetLeader(leader);
                newSteer.SetSlowingRadius(arrivalRadius);
            }
        }
    }


    public void EndFollowLeader()
    {
        foreach (BaseSteering steering in steeringBehaviours)
        {
            if (steering as FollowLeader)
            {
                steering.SetActive(false);
             
            }
        }
    }

    public void DeactivateAll()
    {
        foreach (BaseSteering steering in steeringBehaviours)
        {
            steering.SetActive(false);
        }
    }

    private void Update()
    {
        if (!useSteering) return;
        netForce = Vector2.zero;
        foreach(BaseSteering steering in steeringBehaviours)
        {
            if (steering.IsActive())
            {
                steering.SetTarget(parent.GetTarget());
                netForce += steering.CalculateResultantForce();
            }
        
        }

        rb.velocity += netForce;
    }


}
