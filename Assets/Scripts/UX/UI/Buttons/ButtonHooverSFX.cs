using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHooverSFX : MonoBehaviour
{
    [SerializeField] private GameObject audioPlayerPrefab;
    public void PlayOnHooverSFX()
    {
        IAudio audioPlayer = ObjectPoolManager.Spawn(audioPlayerPrefab, transform.position).GetComponent<IAudio>();
        audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound("ButtonHoover"));
        audioPlayer.PlayAtRandomPitch();

    }
}
