using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemies : MonoBehaviour, IHurtable
{
    private void Awake()
    {
        Destroy(gameObject, 0.25f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<IHurtable>().Damage(0, Vector3.zero, 0f);
        }
    }

    void IHurtable.Damage(float damage, Vector3 knockBackDir, float knockBack)
    {

    }
}
