using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public SoundGroup[] soundGroups;
    public static AudioManager instance;
    public float pitchChange;
    private void Awake()
    {
        //Initialise Singleton Instance
        if (instance == false)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        //Create sound class
        foreach (Sound s in sounds)
        {
            //Create audio source or respective sound
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
        foreach (SoundGroup sG in soundGroups)
        {
            ////Create audio source or respective sound
            //s.source = gameObject.AddComponent<AudioSource>();
            //s.source.clip = s.clip;
            //s.source.volume = s.volume;
            //s.source.pitch = s.pitch;
            //s.source.loop = s.loop;
        }
    }


    //Play sound from sound name
    public void Play(string name)
    {
        Sound currentSound = Array.Find(sounds, sound => sound.name == name);
        if (currentSound != null)
        {

            currentSound.source.Play();


        }
        else
        {
            //Debug.Log("Sound of name:" + name + " was not found");
        }
    }


    //Play sound of at random pitch 
    public void PlayAtRandomPitch(string name)
    {
        Sound currentSound = Array.Find(sounds, sound => sound.name == name);
        if (currentSound != null)
        {
            float ogPitch = currentSound.pitch;
            currentSound.pitch = UnityEngine.Random.Range( currentSound.pitch- pitchChange, currentSound.pitch+ pitchChange);

            currentSound.source.Play();
            currentSound.pitch = ogPitch;


        }
        else
        {
            //Debug.Log("Sound of name:" + name + " was not found");
        }
    }

    //Stop a currently playing sound
    public void Stop(string name)
    {
        Sound currentSound = Array.Find(sounds, sound => sound.name == name);
        if (currentSound != null)
        {
            currentSound.source.Stop();
        }
        else
        {
            //Debug.Log("Sound of name:" + name + " was not found");
        }
    }
}
