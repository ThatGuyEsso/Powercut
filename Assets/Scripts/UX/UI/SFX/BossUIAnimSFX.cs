using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossUIAnimSFX : MonoBehaviour
{
    [SerializeField] private GameObject audioPlayerPrefab;

    [SerializeField] protected string barScaleSFX;
    [SerializeField] protected string healthBarLoadSFX;
    [SerializeField] protected string healthFinishedSFX;
    [SerializeField] private Animator anim;
    public void PlayUIScaleSFX()
    {
        AudioManager.instance.Play(barScaleSFX);
    }

    public void PlayHealthLoadInSFX()
    {
        IAudio audioPlayer = ObjectPoolManager.Spawn(audioPlayerPrefab, transform.position).GetComponent<IAudio>();
        audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound(healthBarLoadSFX));
        audioPlayer.Play();
    }

    public void PlayHealthLoadedSFX()
    {
        IAudio audioPlayer = ObjectPoolManager.Spawn(audioPlayerPrefab, transform.position).GetComponent<IAudio>();
        audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound(healthFinishedSFX));
        audioPlayer.Play();
    }
    public void EndAnimation()
    {
        anim.enabled = false;
    }

}
