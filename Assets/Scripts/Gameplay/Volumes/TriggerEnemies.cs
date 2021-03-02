using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnemies : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<BaseEnemy>().SetTarget(FindObjectOfType<PlayerBehaviour>().transform);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(BeginRecycle());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    private IEnumerator BeginRecycle()
    {
        yield return new WaitForSeconds(3.0f);
        ObjectPoolManager.Recycle(gameObject);
    }

  
}
