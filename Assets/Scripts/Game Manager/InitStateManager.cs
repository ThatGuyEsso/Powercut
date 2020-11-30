using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InitStates
{
    Init,
    GenerateDungeon,
    GenerateRooms,
    SpawnPlayer,
    GameRunning,
    DungeonClear,
    PlayerDead,
    CalculateScores,
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

        //AudioManager.instance.BindToInitManager();
        //TransitionManager.instance.BindToInitManager();

        OnStateChange?.Invoke(currInitState);

    }

    public void BeginNewState(InitStates newState)
    {
        switch (newState)
        {
            case InitStates.GenerateDungeon:
                //GameManager.instance.BindToInitManager();
                //gameState = newState;
                //OnStateChange?.Invoke(gameState);
                break;
            case InitStates.SpawnPlayer:
                //gameState = newState;
                //OnStateChange?.Invoke(gameState);
                break;
            case InitStates.GameRunning:
                //LoadingScreen.instance.ToggleScreen(false);
                break;

        }
    }
}
