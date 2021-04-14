using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    [SerializeField] private Record currentRecord;
    private int currrentTrackList;
    [SerializeField] private AudioSource primarySource;
    [SerializeField] private AudioSource secondarySource;

    [SerializeField] private float crossFadeRate;
    [SerializeField] private float fadeAmount;
    [SerializeField] private AudioMixerGroup musicAudioGroup;
    private bool isAwake;

    public void Awake()
    {

        currentRecord = InitStateManager.instance.GetRecord();
        if (instance == false)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        primarySource.clip = null;
        secondarySource.clip = null;
        primarySource.Stop();
        secondarySource.Stop();
        //Initialise variables
        currrentTrackList = 0;

        //Subscribe to intiation manager
        BindToInitManager();
        isAwake = true;
        if (SaveData.current != null)
        {
            musicAudioGroup.audioMixer.SetFloat("Volume", SaveData.current.soundSettings.music);
          
        }


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

    public void PlayFirstTrack()
    {
      

        if (!primarySource.isPlaying)
        {
            Sound newSong = currentRecord.records[currrentTrackList].StartTrackList();
            primarySource.outputAudioMixerGroup = newSong.mixerGroup;
            primarySource.volume = newSong.volume;
            primarySource.clip = newSong.clip;
            primarySource.pitch = newSong.pitch;
            primarySource.enabled = true;
            primarySource.Play();
            StopAllCoroutines();
            BeginFadeIn();
            StartCoroutine(ListenForSongEnd());
            Debug.Log("Play");

        }
        else
        {
            StopAllCoroutines();
            BeginCrossFade();
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
            if (fadeIn)
            {
                StopAllCoroutines();
                BeginFadeIn();
            }
            StartCoroutine(ListenForSongEnd());


        }
        else
        {
            StopAllCoroutines();
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
        StartCoroutine(FadeOut());
    }
    public void BeginFadeIn()
    {
        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
       
        float maxVolume = primarySource.volume;
        primarySource.volume = 0f;
        yield return new WaitForSeconds(1.0f);
   
        while (primarySource.volume < maxVolume)
        {
            yield return new WaitForSeconds(crossFadeRate);

            primarySource.volume += fadeAmount;


        }
        primarySource.volume = maxVolume;
    }
    private IEnumerator FadeOut()
    {

        float maxVolume = primarySource.volume;
        primarySource.volume = 0f;


        while (primarySource.volume > 0.0f)
        {
            yield return new WaitForSeconds(crossFadeRate);

            primarySource.volume -= fadeAmount;


        }
        primarySource.Stop();
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
        secondarySource.outputAudioMixerGroup = newSong.mixerGroup;
        secondarySource.volume = 0f;
        secondarySource.clip = newSong.clip;
        secondarySource.pitch = newSong.pitch;
        secondarySource.Play();
        while (secondarySource.volume < maxVolume)
        {
            yield return new WaitForSeconds(crossFadeRate);

            primarySource.volume -= fadeAmount*1.5f;
            secondarySource.volume += fadeAmount;


        }
        secondarySource.volume = maxVolume;
        primarySource = secondarySource;
        secondarySource.clip = null;

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
            case InitStates.LoadTitleScreen:
                if (isAwake)
                {
                    BeginFadeOut();
                }
           
                break;

            case InitStates.LoadMainMenu:
                if (isAwake)
                {
                    BeginFadeOut();
                }

                break;
            case InitStates.TitleScreen:
                if (isAwake)
                {
                    currentRecord = InitStateManager.instance.GetRecord();
                    PlayFirstTrack();
                }
                break;
            case InitStates.MainMenu:
                if (isAwake)
                {
                    currentRecord = InitStateManager.instance.GetRecord();
                    PlayFirstTrack();
                }
                break;
            case InitStates.LevelLoaded:
                if (isAwake)
                {
                    StopAllCoroutines();
                    BeginFadeOut();
                    currentRecord = GameStateManager.instance.GetRecord();
                    BindToGameState();
                }
                break;
                
            case InitStates.PlayerDead:
                if (isAwake)
                {
                    BeginFadeOut();
                }
                break;
            case InitStates.RespawnPlayer:
                if (isAwake)
                {
                    PlayNextTrack(true);
                }
                break;

            case InitStates.ExitLevel:
                BeginFadeOut();
                break;
        }
    }

    public void BindToGameState()
    {
        GameStateManager.instance.OnGameStateChange += OnNewGameState;
    }

    public void OnNewGameState(GameStates newstate)
    {
        switch (newstate)
        {
            case GameStates.LevelClear:
                StopAllCoroutines();
                GameStateManager.instance.OnGameStateChange -= OnNewGameState;
                break;

            case GameStates.MainPowerOff:
                if (!primarySource.isPlaying)
                {
                    StopAllCoroutines();
                    PlayNextTrack(true);
                }
             
                break;

        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        InitStateManager.instance.OnStateChange -= EvaluateNewState;
 
    }
}




