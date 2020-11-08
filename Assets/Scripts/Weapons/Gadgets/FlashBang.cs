using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBang : MonoBehaviour
{
    public GameObject flashZonePrefab;
    
    public float timeBeforeExplosion;

    public float shakeMag, ShakeDur, ShakeIn, ShakeOut;
    
    private void FixedUpdate()
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

    private void Explode()
    {
        Instantiate(flashZonePrefab, transform.position, transform.rotation);
        CamShake.instance.DoScreenShake(ShakeDur, shakeMag, ShakeIn, ShakeOut);
        Destroy(gameObject);
    }

    public void LaunchFlashBang(Vector2 dir, float force)
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(dir*force,ForceMode2D.Impulse);
    }
}
