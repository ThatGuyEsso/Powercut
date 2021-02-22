using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour, IShootable, IHurtable,IAudio
{
    public LayerMask collisionLayers;

    public GameObject sparkPrefab;
    public GameObject triggerEnemyPrefab;
    private float damage;
    private float knockBack;
    private Rigidbody2D rb;
    private AudioSource source;
    [SerializeField] protected GameObject audioPlayerPrefab;
    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        source = gameObject.GetComponent<AudioSource>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            IAudio audioPlayer = ObjectPoolManager.Spawn(audioPlayerPrefab, transform.position).GetComponent<IAudio>();
            audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound("BulletCollisionSFX"));
            audioPlayer.PlayAtRandomPitch();
  
            ObjectPoolManager.Spawn(sparkPrefab, transform.position, transform.rotation);
            //SetupAndPlayBulletSound("BulletCollisionSFX");

            ObjectPoolManager.Recycle(gameObject);
        }
        if (other.gameObject.CompareTag("Enemy")|| other.gameObject.CompareTag("PhysicsObject"))
        {
            if (other.GetComponent<IHurtable>() != null)
            {
                other.GetComponent<IHurtable>().Damage(damage, rb.velocity.normalized, knockBack);

            }
            ObjectPoolManager.Spawn(sparkPrefab, transform.position, transform.rotation);
            ObjectPoolManager.Spawn(triggerEnemyPrefab, transform.position, Quaternion.identity);
            ObjectPoolManager.Recycle(gameObject);
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

    public void SetUpAudioSource(Sound sound)
    {
        throw new System.NotImplementedException();
    }

    public void Play()
    {
        throw new System.NotImplementedException();
    }

    public void PlayAtRandomPitch()
    {
        throw new System.NotImplementedException();
    }
}
