using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBang : MonoBehaviour
{
    public GameObject flashZonePrefab;
    
    public float timeBeforeExplosion;

    public float shakeMag, ShakeDur, ShakeIn, ShakeOut;

    [SerializeField] private LayerMask reflectionLayers;

    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (timeBeforeExplosion <= 0)
        {
            Explode();
        }
        else
        {
            timeBeforeExplosion -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy")){
            rb.velocity = Vector2.zero;
        }
        else if (((1 << other.gameObject.layer) & reflectionLayers) != 0)
        {
           ContactPoint2D contactPoint = other.GetContact(0);
            Vector2 dir = Vector2.Reflect(rb.velocity.normalized, contactPoint.normal);
            PerformBounce(dir);
        }
     
    }

    private void PerformBounce(Vector2 dir)
    {
        float speed = rb.velocity.magnitude;
     
        rb.velocity = dir * speed;
    }
    private void Explode()
    {
        FindObjectOfType<FlashZone>().Flash(transform.position, transform.up);
        CamShake.instance.DoScreenShake(ShakeDur, shakeMag, ShakeIn, ShakeOut);
        Destroy(gameObject);
    }

    public void LaunchFlashBang(Vector2 dir, float force)
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(dir*force,ForceMode2D.Impulse);
    }
}
