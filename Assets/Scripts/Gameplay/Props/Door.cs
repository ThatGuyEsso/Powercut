using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IHurtable
{
    Rigidbody2D doorRB;

    private void Awake()
    {
        doorRB = gameObject.GetComponent<Rigidbody2D>();
    }

    public void Damage(float damage, Vector3 knockBackDir, float knockBack)
    {
        doorRB.AddForce(knockBackDir * knockBack, ForceMode2D.Impulse);
    }
}
