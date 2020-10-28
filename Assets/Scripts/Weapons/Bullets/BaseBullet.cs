using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour, IShootable, IHurtable
{
    public LayerMask collisionLayers;

    public GameObject sparkPrefab;
    private float damage;
    private float knockBack;
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            Instantiate(sparkPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<IHurtable>().Damage(damage, rb.velocity.normalized, knockBack);
            Instantiate(sparkPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void IShootable.SetUpBullet(float knockBack, float damage)
    {
        this.damage = damage;
        this.knockBack = knockBack;
    }

    void IHurtable.Damage(float damage, Vector3 knockBackDir, float knockBack)
    {
        //blank just needs to interface with enemy
    }
}
