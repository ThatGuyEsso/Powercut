using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class NavMeshPathfinding : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent navAgent;

    protected Transform target;
    [SerializeField] protected float pathRefreshRate =1.0f;
    [SerializeField] protected float minPathRefreshRate = 0.5f;
    protected WaitForSeconds timeToRefresh;
    public void Init()
    {
        navAgent = gameObject.GetComponent<NavMeshAgent>();

        //navmesh2D values
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;

        navAgent.autoRepath = true;
        float time = Random.Range(pathRefreshRate, minPathRefreshRate);
        timeToRefresh = new WaitForSeconds(time);
    }
    public void SetDestination()
    {
        if(target)
            navAgent.SetDestination(target.position);
    }

    public void Stop()
    {
        navAgent.isStopped = true;
        StopAllCoroutines();
    }

    public void StartAgent(Transform target)
    {
     
        if (navAgent.enabled&&gameObject.activeSelf)
        {
            this.target = target;
            navAgent.isStopped = false;
            StartCoroutine(CalculatePath());
        }

    }


    private IEnumerator CalculatePath()
    {
        if (target && navAgent.isStopped == false) SetDestination();

        yield return timeToRefresh;

        if (target && navAgent.isStopped == false&& navAgent.enabled) StartCoroutine(CalculatePath());



    }
}
