using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : BasePickUp
{


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<IPickUp>().PickUpAmmo();
            ObjectPoolManager.Recycle(gameObject);
        }
    }

 
}
