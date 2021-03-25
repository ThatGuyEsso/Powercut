using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public abstract class BasePickUp : MonoBehaviour, IBreakVFX
{

    protected Rigidbody2D rb;
    virtual protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    virtual public void AddBreakForce(Vector2 dir, float force)
    {
        rb.AddForce(dir * force, ForceMode2D.Impulse);
    }



    public void Break(Vector2 dir, float force)
    {
        //Can't break;
    }



}
