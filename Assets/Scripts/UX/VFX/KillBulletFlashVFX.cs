using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class KillBulletFlashVFX : MonoBehaviour
{

    private ParticleSystem ps;
    private bool isDead;

    private void Awake()
    {
        ps = gameObject.GetComponent<ParticleSystem>();
   
    }

    private void OnEnable()
    {
        isDead = false;
        ps.Simulate(0.0f, true, true);
        ps.Play();
    }
    void Update()
    {
        if (!ps.IsAlive())
        {
            isDead = true;
        }
        if (isDead)
        {
            ps.Simulate(0.0f, true, true);
            ObjectPoolManager.Recycle(gameObject);
          
        }
    }

}
