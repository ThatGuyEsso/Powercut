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
    public static bool isGamePaused = false;
    [SerializeField] private RunTimeData runTimeData;
    [SerializeField] private TutorialManager tutorialManager;
    public GameObject[] itemsToInit;
    [SerializeField]
    Transform initSpawnPoint;
    [SerializeField]
    Transform respawnPoint;
    [SerializeField]
    Transform playerTransform;
    private  GameStates currentGameState;
    public event NewGameStateDelegate OnGameStateChange;
    public delegate void NewGameStateDelegate(GameStates newState);
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
            case InitStates.RespawnPlayer:
                RespawnPlayer();

                break;
            case InitStates.PlayerRespawned:
                BeginNewGameState(GameStates.MainPowerOff);

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
        InitStateManager.instance.BeginNewState(InitStates.PlayerRespawned);
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
        if (runTimeData.firstBoot)
        {
            runTimeData.firstBoot = false;
            if (tutorialManager.gameObject != false)
            {
                TutorialManager tutorial = Instantiate(tutorialManager, Vector3.zero, Quaternion.identity);
                tutorial.Init();
            }
      
        }
    }

    private void PlayerSpawnedUpdate()
    {
        InitStateManager.instance.BeginNewState(InitStates.PlayerSpawned);
    }

    public void PauseGame()
    {
        if (!isGamePaused)
        {
            isGamePaused = true;
            Time.timeScale = 0f;
        }
    }
    public void ResumeGame()
    {
        if (isGamePaused)
        {
            isGamePaused = false;
            Time.timeScale = 1f;
        }
    }

    public void BeginNewGameState(GameStates newGameState)
    {
        switch (newGameState)
        {
            case GameStates.MainPowerOn:
                currentGameState = newGameState;
                OnGameStateChange?.Invoke(newGameState);

                break;

            case GameStates.MainPowerOff:
         
                currentGameState = newGameState;
                OnGameStateChange?.Invoke(newGameState);

                break;

            case GameStates.LevelClear:
                currentGameState = newGameState;
                OnGameStateChange?.Invoke(newGameState);
  

                break;
            case GameStates.TasksCompleted:
                currentGameState = newGameState;
                OnGameStateChange?.Invoke(newGameState);

                break;
        }
    }

    private void OnDestroy()
    {
        Destroy(FindObjectOfType<TutorialManager>().gameObject);

    }
}
