using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiveShield : MonoBehaviour,IHurtable
{
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject hiveGFX;
    [SerializeField] private Collider2D shieldCollider;
    [SerializeField] private GameObject audioPlayerPrefab;
    protected AudioSource aSource;
    [SerializeField] protected string sfxName;
    private BroodNest hive;
    private void Awake()
    {
        if (animator)
            animator.enabled = false;
        if (hiveGFX)
            hiveGFX.SetActive(false);

        hive = GetComponentInParent<BroodNest>();
        shieldCollider.enabled = false;

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

    public void ResetShield()
    {
        if (animator)
            animator.enabled = false;
        if (hiveGFX)
            hiveGFX.SetActive(false);
        shieldCollider.enabled = false;
    }
    public void ShieldBuilt()
    {
        animator.enabled = false;
        hive.SpawnBroodDelegates();
    }
    public void ShieldRemoved()
    {
        animator.enabled = false;
        hiveGFX.SetActive(false);
        shieldCollider.enabled = false ;
        hive.EvaluateBossStage();
    }

    public void PlayShieldBuildSFX()
    {
     
        IAudio audioPlayer = ObjectPoolManager.Spawn(audioPlayerPrefab, transform.position).GetComponent<IAudio>();
        audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound("CocoonBuildingSFX"));
        audioPlayer.Play();
    }
    public void PlayShieldFinishedSFX()
    {
        IAudio audioPlayer = ObjectPoolManager.Spawn(audioPlayerPrefab, transform.position).GetComponent<IAudio>();
        audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound("CoonFinishedSFX"));
        audioPlayer.Play();
    }
    public void PlayAnimation(string animName)
    {
        shieldCollider.enabled = true;
        animator.enabled = true;
        hiveGFX.SetActive(true);
        animator.Play(animName);
    }

    public void Damage(float damage, Vector3 knockBackDir, float knockBack)
    {
        if (aSource)
        {
            if (!aSource.isPlaying){
                aSource.Play();
            }
        }

    }

    public void Push(Vector3 knockBackDir, float knockBack)
    {
        if (aSource)
        {
            if (!aSource.isPlaying)
            {
                aSource.Play();
            }
        }
    }
}
