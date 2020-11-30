using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages current state of level so component knows what face were in (I.E Player Death, Game completed, objectives complete)

public enum GameStates
{
    MainPowerOn,
    MainPowerOff,
    TasksCompleted,
    LevelClear
};
public class GameStateManager : MonoBehaviour, IInitialisable
{
    public static GameStateManager instance;
    [SerializeField]
    public GameObject[] itemsToInit;
    [SerializeField]
    Transform initSpawnPoint;
    [SerializeField]
    Transform respawnPoint;
    private  GameStates currentGameState;
    private void Awake()
    {
        
        if (instance == false)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }



    //Starts the game
    public void SwitchPowerOff()
    {
        currentGameState = GameStates.MainPowerOff;

        Debug.Log(currentGameState.ToString());
        FindObjectOfType<PlayerAnimController>().UpdatePlayergun();
        UIManager.instance.eventDisplay.CreateEvent("Main Power Switched off", Color.yellow);
    }
    public void TasksCompleted()
    {
        currentGameState = GameStates.TasksCompleted;
        Debug.Log("Power is off but tasks completed");
        Debug.Log(currentGameState.ToString());
    }

    public void LevelCleared()
    {
        currentGameState = GameStates.LevelClear;
        LevelLampsManager.instance.FixAllSceneLamps();
        Debug.Log("level cleared");
        Debug.Log(currentGameState.ToString());
        UIManager.instance.eventDisplay.CreateEvent("Main Power Switched On", Color.green);
        UIManager.instance.eventDisplay.CreateEvent("Level Cleared", Color.green);
    }
    public void SwitchPowerOn()
    {
        currentGameState = GameStates.MainPowerOn;
        Debug.Log("Power is On");
        Debug.Log(currentGameState.ToString());
        FindObjectOfType<PlayerAnimController>().UpdatePlayergun();
        UIManager.instance.eventDisplay.CreateEvent("Main Power Switched On", Color.green);
    }

    public GameStates GetCurrentGameState()
    {
        return currentGameState;
    }

    public void BindToInitManager()
    {
        InitStateManager.instance.OnStateChange += EvaluateNewState;
    }

    private void EvaluateNewState(InitStates newState)
    {
        switch (newState)
        {
            case InitStates.SpawnPlayer:
                SpawnPlayer();
                break;
        }
    }
    private void SpawnPlayer()
    {
        FindObjectOfType<PlayerBehaviour>().transform.position = initSpawnPoint.position;
        Invoke("PlayerSpawnedUpdate", 0.5f);
      
    }
    private void RespawnPlayer()
    {
        FindObjectOfType<PlayerBehaviour>().transform.position = respawnPoint.position;
        InitStateManager.instance.BeginNewState(InitStates.PlayerSpawned);
    }

    void IInitialisable.Init()
    {
        Init();
    }
    public void Init()
    {
        foreach(GameObject init in itemsToInit)
        {
            init.GetComponent<IInitialisable>().Init();
        }
    }

    private void PlayerSpawnedUpdate()
    {
        InitStateManager.instance.BeginNewState(InitStates.PlayerSpawned);
    }
}
