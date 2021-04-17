using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingDrones : MonoBehaviour,IHurtable
{
    private Rigidbody2D rb;
    private Transform target;
    [SerializeField] private float maxSpeed;
    private float damage;
    private float knockBack;
    private bool isActive;
    private AttackDrones owner;
    [SerializeField] private float tickRate;
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private GameObject audioPlayerPrefab;

    private AudioSource aSource;
    [SerializeField] private string sfxName;
    [SerializeField]  private LayerMask collisionLayers;

    public void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
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
    public void SetUpDrone(Transform target, float damage, float knockback, AttackDrones owner)
    {
        this.target = target;
        isActive = true;
        this.damage = damage;
        knockBack = knockback;
        this.owner = owner;
        if (aSource) aSource.Play();
        StartCoroutine(UpdateDirection());
    }


    public void CalculateSeekVelocity()
    {
        if (!target) return;
        Vector2 desiredVel = ((Vector2)target.position - rb.position).normalized * Time.deltaTime * maxSpeed;
        if (rb)
            rb.velocity = desiredVel;
    }


    private IEnumerator UpdateDirection()
    {
        if (target&&isActive)
        {
            yield return new WaitForSeconds(tickRate);
            CalculateSeekVelocity();
            StartCoroutine(UpdateDirection());
        }
  

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            IAudio audioPlayer = ObjectPoolManager.Spawn(audioPlayerPrefab, transform.position).GetComponent<IAudio>();
            audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound("BugsSplat"));
            audioPlayer.PlayAtRandomPitch();

            ObjectPoolManager.Spawn(explosionVFX, transform.position, transform.rotation);
            if (owner)
                owner.Dronekilled(this);
            ObjectPoolManager.Recycle(gameObject);
        }
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<IHurtable>() != null)
            {
                other.GetComponent<IHurtable>().Damage(damage, rb.velocity.normalized, knockBack);

            }
            IAudio audioPlayer = ObjectPoolManager.Spawn(audioPlayerPrefab, transform.position).GetComponent<IAudio>();
            audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound("BugsSplat"));
            audioPlayer.PlayAtRandomPitch();
            ObjectPoolManager.Spawn(explosionVFX, transform.position, transform.rotation);
            if (owner)
                owner.Dronekilled(this);
            ObjectPoolManager.Recycle(gameObject);
        }
        if (other.gameObject.CompareTag("Swarm"))
        {
            Vector2 dir = other.transform.position - transform.position;
            other.GetComponent<Rigidbody2D>().AddForce(dir.normalized * 2.0f, ForceMode2D.Impulse);
        }
   
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Swarm"))
        {
            Vector2 dir = other.transform.position - transform.position;
            other.GetComponent<Rigidbody2D>().AddForce(dir.normalized * 2.0f, ForceMode2D.Impulse);
        }
    }

    private void OnDisable()
    {
        if (aSource&&aSource.isPlaying) aSource.Stop();
        StopAllCoroutines();
        isActive = false;
        target = null;
    }

    public void Damage(float damage, Vector3 knockBackDir, float knockBack)
    {
        IAudio audioPlayer = ObjectPoolManager.Spawn(audioPlayerPrefab, transform.position).GetComponent<IAudio>();
        audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound("BugsSplat"));
        audioPlayer.PlayAtRandomPitch();
        ObjectPoolManager.Spawn(explosionVFX, transform.position, transform.rotation);
        if (owner)
            owner.Dronekilled(this);
        ObjectPoolManager.Recycle(gameObject);
    }

    public void Push(Vector3 knockBackDir, float knockBack)
    {
        throw new System.NotImplementedException();
    }
}
