using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSpawner : MonoBehaviour
{
    private Transform target;
    private GameObject enemyToSpawn;
    [SerializeField] private GameObject hatchVFX;
    private Rigidbody2D rb;

    [SerializeField] private float minHatchTime, maxHatchTime;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void SetUpEgg(Transform target, GameObject enemy)
    {
        this.target = target;
        enemyToSpawn = enemy;

        float rand = Random.Range(minHatchTime, maxHatchTime);
        Invoke("Hatch", rand);
    }
    public void EjectEgg(float force, Vector2 dir)
    {
        rb.AddForce(force * dir, ForceMode2D.Impulse);
    }
    public void Hatch()
    {
        ObjectPoolManager.Spawn(hatchVFX, transform.position, Quaternion.identity);
       BaseEnemy enemy = ObjectPoolManager.Spawn(enemyToSpawn, transform.position, Quaternion.identity).GetComponent<BaseEnemy>();
       enemy.SetTarget(target);
        ObjectPoolManager.Recycle(this);

    }
    
}
