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
    RespawnPlayer,
    PlayerRespawned,
    PlayerDead,
    PlayerSpawned,
    GameRunning,
    LevelClear,
    ExitLevel,
    TitleScreen

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
                OnStateChange?.Invoke(newState);
                Debug.Log("Player spawned");
                LoadingScreen.instance.cam.SetActive(false);
                LoadingScreen.instance.BeginFade(false);
                currInitState = InitStates.GameRunning;
                OnStateChange?.Invoke(currInitState);
         
                break;
            case InitStates.GameRunning:
                OnStateChange?.Invoke(newState);
                //LoadingScreen.instance.ToggleScreen(false);
                break;

            case InitStates.LevelLoaded:

                currInitState = newState;
                OnStateChange?.Invoke(currInitState);
                GameStateManager.instance.BindToInitManager();

                break;
            case InitStates.ExitLevel:

                LoadingScreen.instance.ToggleScreen(true);
                currInitState = newState;
                OnStateChange?.Invoke(currInitState);

                break;
            case InitStates.TitleScreen:

                LoadingScreen.instance.BeginFade(false);
                currInitState = newState;
                OnStateChange?.Invoke(currInitState);

                break;
            case InitStates.PlayerSceneLoaded:
                OnStateChange?.Invoke(newState);
                FindObjectOfType<WeaponManager>().Init();
                FindObjectOfType<LightManager>().BindToInitManager();
                FindObjectOfType<PlayerBehaviour>().BindToInitManager();
                WeaponManager.instance.BindToInitManager();
                GameStateManager.instance.Init();
   
                currInitState = InitStates.SpawnPlayer;
                OnStateChange?.Invoke(currInitState);
                break;
            case InitStates.UISceneLoaded:
                currInitState = newState;
                PauseScreen.instance.Resume();
                UIManager.instance.Init();
                UIManager.instance.BindToInitManager();
                OnStateChange?.Invoke(currInitState);
                break;
            case InitStates.PlayerDead:
                currInitState = newState;
                OnStateChange?.Invoke(currInitState);

                break;
            case InitStates.RespawnPlayer:
                LoadingScreen.instance.ToggleScreen(true);
                currInitState = newState;
                OnStateChange?.Invoke(currInitState);
                break;
            case InitStates.PlayerRespawned:
                currInitState = newState;
                OnStateChange?.Invoke(currInitState);
                LoadingScreen.instance.BeginFade(false);
                currInitState = InitStates.GameRunning;
                OnStateChange?.Invoke(currInitState);
                break;
        }
    }
}
