using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PheremoneBlast : BaseAttackPattern
{
    protected Collider2D attackCollider;
    [SerializeField]protected ParticleSystem vfx;
    private void Awake()
    {
        attackCollider = gameObject.GetComponent<Collider2D>();
        if(attackCollider)
            attackCollider.enabled = false;
        vfx.Simulate(0.0f, true, true);
        vfx.gameObject.SetActive(false);

        aSource = GetComponent<AudioSource>();

        if (aSource)
        {
            Sound droneSFX = AudioManager.instance.GetSound(sfxName);

            aSource.clip = droneSFX.clip;
            aSource.volume = droneSFX.volume;
            aSource.outputAudioMixerGroup = droneSFX.mixerGroup;
            aSource.pitch = droneSFX.pitch;
            aSource.loop = droneSFX.loop;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<IHurtable>() != null)
            {
                Vector2 dir = other.transform.position - transform.position;
                other.GetComponent<IHurtable>().Damage(damage, dir.normalized, knockBack);

            }
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<IHurtable>() != null)
            {
                Vector2 dir = other.transform.position - transform.position;
                other.GetComponent<IHurtable>().Damage(damage, dir.normalized, knockBack);

            }
        }
    }
    protected override IEnumerator BeginAttackCycle()
    {
        yield return new WaitForSeconds(attackRate);
       
        ExecuteAttack();

   
        if (isRunning)
        {
            StartCoroutine(BeginAttackCycle());

        }
    }
   
        //if not in range wait till player is in range
    
    public override void ExecuteAttack()
    {

        attackCollider.enabled = true;
        vfx.gameObject.SetActive(true);
        vfx.Simulate(0.0f, true, true);
        vfx.Play();
        if (gameObject.activeInHierarchy && aSource) aSource.Play();
        StartCoroutine(ListenToEndOfVFX());
    }
    public override void StopRunning()
    {
        base.StopRunning();
        attackCollider.enabled = false;
        StopAllCoroutines();
    }

    public IEnumerator ListenToEndOfVFX()
    {
        while (vfx.IsAlive())
        {
            yield return null;
        }
        vfx.gameObject.SetActive(false);
        attackCollider.enabled = false;
    }
}
