using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public SoundGroup[] soundGroups;
    public static AudioManager instance;
    public float pitchChange;

    [SerializeField] private AudioMixerGroup uiGroup, soundEffectGroup;
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
        DontDestroyOnLoad(gameObject);

        //Create sound class
        foreach (Sound s in sounds)
        {
            //Create audio source or respective sound
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.mixerGroup;
        }
        for(int i=0;i<soundGroups.Length;i++)
        {
            ////Create audio source for each sound group
            soundGroups[i].source = gameObject.AddComponent<AudioSource>();
            soundGroups[i].source.volume = soundGroups[i].volume;
            soundGroups[i].source.pitch = soundGroups[i].pitch;
            soundGroups[i].source.loop = soundGroups[i].loop;
            soundGroups[i].source.outputAudioMixerGroup = soundGroups[i].mixerGroup;
        }
        UpdateSoundLevels();
    }

    public void PlayRandFromGroup(string groupName)
    {
        //Find Sound Group
        SoundGroup soundGroup = Array.Find(soundGroups, group => group.name == groupName);

        //load new clip into source
        soundGroup.source.clip = soundGroup.GetRandClip();

        if(soundGroup != null)
        {
            //Play new sound if it exists
            soundGroup.source.Play();
        }
        else
        {
            Debug.Log("Group of name:" + groupName + " was not found");
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
            Debug.Log("Sound of name:" + name + " was not found");
        }
    }

    public void PlayIfFree(string name)
    {
        Sound currentSound = Array.Find(sounds, sound => sound.name == name);
        if (currentSound != null)
        {

            if (!currentSound.source.isPlaying) currentSound.source.Play();


        }
        else
        {
            Debug.Log("Sound of name:" + name + " was not found");
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

    public Sound GetSound(string name)
    {
        Sound currentSound = Array.Find(sounds, sound => sound.name == name);
        return currentSound;
    }
    public Sound GetSound(Sound sound)
    {
        foreach(Sound currentSound in sounds)
        {
            if (currentSound == sound) return currentSound;
        }
        return null;
    }
    public float GetRandomPitchOfSound(Sound sound)
    {
        return UnityEngine.Random.Range(sound.pitch - pitchChange, sound.pitch + pitchChange);
    }
    public void BindToInitManager()
    {
        InitStateManager.instance.OnStateChange += EvaluateNewState;
    }

    private void EvaluateNewState(InitStates newState)
    {
        switch (newState)
        {
            case InitStates.Init:
            
                break;
        }
    }



    private void UpdateSoundLevels()
    {
        if (SaveData.current != null)
        {
            uiGroup.audioMixer.SetFloat("Volume", SaveData.current.soundSettings.uiEffect);
            soundEffectGroup.audioMixer.SetFloat("Volume", SaveData.current.soundSettings.soundEffect);
        }
    }
}
