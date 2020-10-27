using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum EnemyStates
{
    Wander,
    Chase,
    Attack,
    Destroy,
    Idle
};
[RequireComponent(typeof(Rigidbody2D))]
public abstract class BaseEnemy : MonoBehaviour
{
    //States
    protected EnemyStates currentState;
    protected bool isHurt;
    protected bool isTargetHuman;

    //Settings
    public EnemySettings settings;
    protected float smoothRot;
    //Component refs
    protected Rigidbody2D rb;

    //Pathfinding
    protected int currentWaypoint = 0;
    public Transform target;
    protected Path path;
    protected Seeker seeker;
    protected Vector3 moveDirection;
    protected float nextWaypointDistance = 3f;
    //stas
    protected float currentHealth;
    private float smoothAX;
    private float smoothAY;
    private float smoothDX;
    private float smoothDY;

    virtual protected void Awake()
    {
        //cache component references
        rb = gameObject.GetComponent<Rigidbody2D>();
        currentHealth = settings.maxHealth;
        seeker = gameObject.GetComponent<Seeker>();
       // InvokeRepeating("ProcessAI", 0f, settings.aiTickrate);
    }




    virtual protected void ProcessAI()
    {
        //Require own AI implementation
    }
    //#MovementFunction FUNCTIONS#
    virtual protected void SmoothAccelerate(Vector3 direction, float maxSpeed, float rate)
    {
        Vector3 targetVelocity = Vector2.zero;

        targetVelocity.x = Mathf.SmoothDamp(rb.velocity.x, maxSpeed * direction.x, ref smoothAX, rate);
        targetVelocity.y = Mathf.SmoothDamp(rb.velocity.y, maxSpeed * direction.y, ref smoothAY, rate);
      
        rb.velocity = targetVelocity*Time.deltaTime;
    }

    virtual protected void SmoothDecelerate(float minSpeed, float rate)
    {
        Vector2 targetVelocity = Vector2.zero;

        if (rb.velocity.magnitude <= 0.1f)
        {
            smoothAX = 0f;
            smoothAY = 0f;
            smoothDX = 0f;
            smoothDY = 0f;
        }
        else
        {
            targetVelocity.x = Mathf.SmoothDamp(rb.velocity.x, minSpeed, ref smoothDX, rate);
            targetVelocity.y = Mathf.SmoothDamp(rb.velocity.y, minSpeed, ref smoothDY, rate);
        }
        rb.velocity = targetVelocity;
    }

    virtual protected void FaceMovementDirection()
    {
        float targetAngle = EssoUtility.GetAngleFromVector(rb.velocity.normalized);
       /* targetAngle += 90f;*/// turn offset -Due to converting between forward vector and up vector
        //if (targetAngle < 0) targetAngle += 360f;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.z, targetAngle, ref smoothRot, settings.rotationSpeed);//rotate player smoothly to target angle
        transform.rotation = Quaternion.Euler(0f, 0f, angle);//update angle
        //fovObject.SetAimDirection((-1)*fovObject.GetVectorFromAngle(angle));
    }

    //#End MovementFunction FUNCTIONS#




    //#PATHFINDING FUNCTIONS#
    virtual protected void UpdatePath()
    {
        if (target != null)
        {
            if (seeker.IsDone()) //if seeker is done mapping, start the path to target
                seeker.StartPath(rb.position, target.position, OnPathComplete);
        }

    }
    virtual protected void OnPathComplete(Path p)
    {
        if (!p.error)  //if no errors, reset path and waypoint
        {
            path = p;
            currentWaypoint = 0;
        }

    }


    virtual protected void DrawPathToTarget()
    {
        float distance = Vector3.Distance(transform.position, target.transform.position);
 
            if (path == null)
            {
                return; //No path so return
            }

            if (currentWaypoint >= path.vectorPath.Count)
            {
                return; // current waypoint is out of range of total way point. Hence path end has been reached return
            }

            distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWaypointDistance) // if the distance to the next waypoint is shorter than the current one, go to it
            {
                currentWaypoint++; // AI cuts corners
            }
            moveDirection = path.vectorPath[currentWaypoint] - transform.position;

    }
    //# END OF PATHFINDING FUNCTIONS#


    //#Setters#
    //Set target of enemy
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    //Set state 
    public void SetEnemyState(EnemyStates newState)
    {
        currentState = newState;
    }
    //# End of Setters#

    protected void ResolveTargetType()
    {
        if(target != null)
        {
            if (target.CompareTag("Player"))
            {
                isTargetHuman = true;
            }
            else
            {
                isTargetHuman = false;
            }

        }
    }
}
