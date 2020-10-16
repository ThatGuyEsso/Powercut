using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour
{
    public LayerMask collisionLayers;
    public GameObject sparkPrefab;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            Instantiate(sparkPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
