using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour, IInitialisable
{
    public static MusicManager instance;
    private Record currentRecord;
    private int currrentTrackList;
    [SerializeField] private AudioSource primarySource;
    [SerializeField] private AudioSource secondarySource;

    [SerializeField] private float crossFadeRate;
    [SerializeField] private float fadeAmount;
    public void Init()
    {
        if (instance == false)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        //Initialise variables
        currrentTrackList = 0;
 
        //Subscribe to intiation manager
        BindToInitManager();

        currentRecord = InitStateManager.instance.GetRecord();
  

    }


    public void StopCurrentSong()
    {
        if(primarySource.clip!=false && primarySource.isPlaying)
        {
            primarySource.Pause();
            
        }
    }

    public void ResumeCurrentSong()
    {
        if (primarySource.clip != false && !primarySource.isPlaying)
        {
            primarySource.Play();

        }
    }

    public void PlayNextTrack(bool fadeIn)
    {
        if (!primarySource.isPlaying)
        {
            Sound newSong = currentRecord.records[currrentTrackList].PlayNextTrack();
            primarySource.outputAudioMixerGroup = newSong.mixerGroup;
            primarySource.volume = newSong.volume;
            primarySource.clip = newSong.clip;
            primarySource.pitch = newSong.pitch;
            primarySource.enabled = true;
            primarySource.Play();
            if (fadeIn) BeginFadeIn();


        }
        else
        {
         
            BeginCrossFade();
        }
    } 

    public IEnumerator BeginToPlayNextTrack()
    {
        float waitTime  = currentRecord.records[currrentTrackList].GetTrackWaitTime();
        yield return new WaitForSeconds(waitTime);
        PlayNextTrack(false);
    }

    public IEnumerator ListenForSongEnd()
    {

        //If there is a song and it is playing wait
        while (primarySource.isPlaying&& primarySource.clip != false)
        {
            yield return null;

        }

        //Finished play next song
        StartCoroutine(BeginToPlayNextTrack());



    }

    public void BeginFadeOut()
    {

    }
    public void BeginFadeIn()
    {

    }

    private IEnumerator FadeIn()
    {
       
        float maxVolume = primarySource.volume;
        primarySource.volume = 0f;
  
   
        while (primarySource.volume < maxVolume)
        {
            yield return new WaitForSeconds(crossFadeRate);

            primarySource.volume += fadeAmount;


        }
        primarySource.volume = maxVolume;
    }
    public void BeginCrossFade()
    {
        StopAllCoroutines();
        StartCoroutine(CrossFade());
    }
    public IEnumerator CrossFade()
    {
        Sound newSong = currentRecord.records[currrentTrackList].GetNextTrack();
        float maxVolume = newSong.volume;
        primarySource.outputAudioMixerGroup = newSong.mixerGroup;
        secondarySource.volume = 0f;
        secondarySource.clip = newSong.clip;
        secondarySource.pitch = newSong.pitch;
        secondarySource.Play();
        while (secondarySource.volume < maxVolume)
        {
            yield return new WaitForSeconds(crossFadeRate);

            primarySource.volume -= fadeAmount;
            secondarySource.volume += fadeAmount;


        }
        secondarySource.volume = maxVolume;
        primarySource = secondarySource;
        secondarySource.Stop();
        currentRecord.records[currrentTrackList].IncrementTrackIndex();
        StartCoroutine(ListenForSongEnd());
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
            case InitStates.TitleScreen:
                PlayNextTrack(false);
                break;
            case InitStates.LoadMainMenu:
              
                break;
            case InitStates.LevelLoaded:

                break;
                
            case InitStates.PlayerDead:
                
                break;
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        InitStateManager.instance.OnStateChange -= EvaluateNewState;
 
    }
}




