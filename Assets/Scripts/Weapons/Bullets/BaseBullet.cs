using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour, IShootable, IHurtable
{
    public LayerMask collisionLayers;

    public GameObject sparkPrefab;
    public GameObject triggerEnemyPrefab;
    private float damage;
    private float knockBack;
    private Rigidbody2D rb;
    private AudioSource source;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        source = gameObject.GetComponent<AudioSource>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            Instantiate(sparkPrefab, transform.position, transform.rotation);
            SetupAndPlayBulletSound("BulletCollisionSFX");
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<IHurtable>().Damage(damage, rb.velocity.normalized, knockBack);
            Instantiate(sparkPrefab, transform.position, transform.rotation);
            Instantiate(triggerEnemyPrefab, transform.position, Quaternion.identity);
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

    public void SetupAndPlayBulletSound(string clipName)
    {
       Sound bulletSound = AudioManager.instance.GetSound(clipName);
        //Create audio source or respective sound

        source.clip = bulletSound.clip;
        source.volume = bulletSound.volume;
        source.pitch = AudioManager.instance.GetRandomPitchOfSound(bulletSound);
        source.loop = bulletSound.loop;
        source.outputAudioMixerGroup = bulletSound.mixerGroup;

        source.Play();

        
    }
}
