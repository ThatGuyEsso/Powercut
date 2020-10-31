using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemies : MonoBehaviour
{
    private void Awake()
    {
        Destroy(gameObject, 0.25f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<BaseEnemy>().SetTarget(FindObjectOfType<PlayerBehaviour>().transform);
        }
    }

}
