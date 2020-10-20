using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class KillBulletFlashVFX : MonoBehaviour
{
    public float timeToDim;
    private ParticleSystem ps;
    private bool isDead;
    private Light2D flash;
    private void Awake()
    {
        ps = gameObject.GetComponent<ParticleSystem>();
        flash = gameObject.GetComponentInChildren<Light2D>();
    }
    void Update()
    {
        if (!ps.IsAlive())
        {
            isDead = true;
        }
        if (isDead)
        {
            //flash.intensity = Mathf.Lerp(flash.intensity, 0f, timeToDim);
            //if (flash.intensity <= 0f)
            //{
                Destroy(gameObject);
            //}
        }
    }

}
