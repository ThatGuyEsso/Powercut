using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBullet : MonoBehaviour, IShootable, IHurtable,IAudio
{
    public LayerMask collisionLayers;

    public GameObject sparkPrefab;
    public GameObject triggerEnemyPrefab;
    protected float damage;
    protected float knockBack;
    protected float shotForce;
    protected Rigidbody2D rb;
    protected AudioSource source;
    [SerializeField] protected GameObject audioPlayerPrefab;
    [SerializeField] protected string targetTag;
    virtual protected void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        source = gameObject.GetComponent<AudioSource>();
    }
    virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        if(((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            IAudio audioPlayer = ObjectPoolManager.Spawn(audioPlayerPrefab, transform.position).GetComponent<IAudio>();
            audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound("BulletCollisionSFX"));
            audioPlayer.PlayAtRandomPitch();

            Vector2 backDir = rb.velocity.normalized * -1;
            Vector2 pos = rb.position + backDir*0.6f;
            ObjectPoolManager.Spawn(sparkPrefab, pos, transform.rotation);
            //SetupAndPlayBulletSound("BulletCollisionSFX");

            ObjectPoolManager.Recycle(gameObject);
        }
        if (other.gameObject.CompareTag(targetTag) || other.gameObject.CompareTag("PhysicsObject"))
        {
            if (other.GetComponent<IHurtable>() != null)
            {
                other.GetComponent<IHurtable>().Damage(damage, rb.velocity.normalized, knockBack);

            }
            ObjectPoolManager.Spawn(sparkPrefab, transform.position, transform.rotation);
            if(triggerEnemyPrefab)
                ObjectPoolManager.Spawn(triggerEnemyPrefab, transform.position, Quaternion.identity);
            ObjectPoolManager.Recycle(gameObject);
        }

        if (other.gameObject.CompareTag("Swarm"))
        {
            if (other.GetComponent<IHurtable>() != null)
            {
                other.GetComponent<IHurtable>().Damage(damage, rb.velocity.normalized, knockBack);

            }

            if (triggerEnemyPrefab)
                ObjectPoolManager.Spawn(triggerEnemyPrefab, transform.position, Quaternion.identity);
            ObjectPoolManager.Recycle(gameObject);
        }
    }

    virtual public void SetUpBullet(float knockBack, float damage)
    {
        this.damage = damage;
        this.knockBack = knockBack;
    }

    public void Damage(float damage, Vector3 knockBackDir, float knockBack)
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

    virtual public void OnEnable()
    {
        StartCoroutine(RecycleAfterTime());
    }

    virtual public void OnDisable()
    {
        StopAllCoroutines();
    }
    virtual public IEnumerator RecycleAfterTime()
    {
        yield return new WaitForSeconds(3.0f);
        ObjectPoolManager.Recycle(gameObject);
    }

    virtual public void SetUpAudioSource(Sound sound)
    {
        throw new System.NotImplementedException();
    }

    virtual public void Play()
    {
        throw new System.NotImplementedException();
    }

    virtual public void PlayAtRandomPitch()
    {
        throw new System.NotImplementedException();
    }

    virtual public void Push(Vector3 knockBackDir, float knockBack)
    {
        throw new System.NotImplementedException();
    }

    public void Shoot(Vector2 dir, float force)
    {
        rb.AddForce(dir * force, ForceMode2D.Impulse);
        shotForce = force;
    }
}
