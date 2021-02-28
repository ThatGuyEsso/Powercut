using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashZone : MonoBehaviour,IHurtable
{
    private void Awake()
    {
        Destroy(gameObject, 0.25f);
    }

    void IHurtable.Damage(float damage, Vector3 knockBackDir, float knockBack)
    {

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 dir = transform.position -collision.transform.position;
            collision.gameObject.GetComponent<IHurtable>().Damage(200f, dir.normalized, 2000f);
        }
    }
}
