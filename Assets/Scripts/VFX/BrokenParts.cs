using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BrokenParts : MonoBehaviour,IBreakVFX
{
    Rigidbody2D rb;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
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
