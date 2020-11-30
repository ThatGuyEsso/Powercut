using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InitStates
{
    Init,
    InitLevel,
    LevelLoaded,
    PlayerSceneLoaded,
    UISceneLoaded,
    SpawnPlayer,
    PlayerSpawned,
    GameRunning,
    LevelClear,
    PlayerDead,
    ExitLevel

};

public class InitStateManager : MonoBehaviour
{
    public static InitStateManager instance;
    public GameObject audioManager, transitionManager;
    public static InitStates currInitState;

    public event NewInitStateDelegate OnStateChange;
    public delegate void NewInitStateDelegate(InitStates newState);
    private void Awake()
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
    }
    private void Start()
    {
        Init();
    }

    public void Init()
    {
        //Initial state
        currInitState = InitStates.Init;
        //Spawn necessary utillity managers
        Instantiate(audioManager, Vector3.zero, Quaternion.identity);
        Instantiate(transitionManager, Vector3.zero, Quaternion.identity);

        AudioManager.instance.BindToInitManager();
        TransitionManager.instance.BindToInitManager();

        OnStateChange?.Invoke(currInitState);

    }

    public void BeginNewState(InitStates newState)
    {
        switch (newState)
        {
            case InitStates.InitLevel:
                GameStateManager.instance.BindToInitManager();
                currInitState = newState;
                OnStateChange?.Invoke(currInitState);
                break;
            case InitStates.PlayerSpawned:
                FindObjectOfType<PlayerBehaviour>().Init();

                LoadingScreen.instance.cam.SetActive(false);
                LoadingScreen.instance.BeginFade(false);
                currInitState = InitStates.GameRunning;
                OnStateChange?.Invoke(currInitState);
         
                break;
            case InitStates.GameRunning:
                LoadingScreen.instance.ToggleScreen(false);
                break;

            case InitStates.LevelLoaded:

                currInitState = newState;
                OnStateChange?.Invoke(currInitState);
                GameStateManager.instance.BindToInitManager();

                break;
            case InitStates.PlayerSceneLoaded:
                FindObjectOfType<WeaponManager>().Init();
                FindObjectOfType<LightManager>().Init();
      
                GameStateManager.instance.Init();
   
                currInitState = InitStates.SpawnPlayer;
                OnStateChange?.Invoke(currInitState);
                break;
            case InitStates.UISceneLoaded:
                currInitState = newState;
                OnStateChange?.Invoke(currInitState);

                break;
        }
    }
}
