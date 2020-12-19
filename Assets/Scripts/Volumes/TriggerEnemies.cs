using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemies : MonoBehaviour
{
    private void Awake()
    {
        Invoke("BeginRecycle", 0.15f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<BaseEnemy>().SetTarget(FindObjectOfType<PlayerBehaviour>().transform);
        }
    }

    public void BeginRecycle()
    {
        ObjectPoolManager.Recycle(gameObject);
    }
}
