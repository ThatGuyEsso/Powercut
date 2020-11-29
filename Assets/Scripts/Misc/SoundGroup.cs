using UnityEngine.Audio;
using UnityEngine;


[System.Serializable]
public class SoundGroup 
{
    public string name;
    public AudioClip[] clips;

    [Range(0, 1f)]
    public float volume;
    [Range(0.1f, 3f)]
    public float pitch;
    public bool loop;
    [HideInInspector]
    public AudioSource source;
}
