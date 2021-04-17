using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BrokenParts : MonoBehaviour,IBreakVFX
{
    Rigidbody2D rb;
    Collider2D partCollider;
    [SerializeField] private float stopRate;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        partCollider = gameObject.GetComponent<Collider2D>();
    }


    private void Update()
    {
     

        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, Time.deltaTime * stopRate);

        if (rb.velocity.magnitude <= 1f)
        {
            rb.velocity = Vector2.zero;
            enabled = false;
        }

    }


    private void OnEnable()
    {
        enabled = true;
    }
    public void Break(Vector2 dir, float force)
    {
        throw new System.NotImplementedException();
    }

    public void AddBreakForce(Vector2 dir, float force)
    {
        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }



}
