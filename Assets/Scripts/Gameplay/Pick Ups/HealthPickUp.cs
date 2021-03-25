using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : BasePickUp
{
    [SerializeField] private float health;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IPickUp>().PickUpHealth(health);
            ObjectPoolManager.Recycle(gameObject);
        }
    }
}
