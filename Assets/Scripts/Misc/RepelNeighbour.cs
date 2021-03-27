using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepelNeighbour : MonoBehaviour
{
    [SerializeField] private string targets;
    [SerializeField] private float pushForce;
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targets))
        {
            other.GetComponent<IHurtable>().Push(CalculatePushDir(other.transform), pushForce);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(targets))
        {
            other.GetComponent<IHurtable>().Push(CalculatePushDir(other.transform), pushForce);
        }
    }


    public Vector2 CalculatePushDir(Transform target)
    {
        Vector2 dir = (target.position - transform.position).normalized;
        return dir;
    }
}
