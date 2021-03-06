using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameModes
{
    Menu,
    Powercut,
    Dialogue,
    Cutscene,
    Debug,
};
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
    Credits,
    TitleScreen,
    LoadTitleScreen,
    MainMenu,
    LoadMainMenu
};

public class InitStateManager : MonoBehaviour
{
    public static InitStateManager instance;
    [SerializeField] Record record;
    public GameObject audioManager, transitionManager,objectPoolManager,clientManager,musicManager;
    public static InitStates currInitState;
    public static GameModes currGameMode;
    [SerializeField] private RunTimeData runTimeData;

    public event NewInitStateDelegate OnStateChange;
    public delegate void NewInitStateDelegate(InitStates newState);

    public static string SaveName = "/Powercut";
    public Action OnContinue;

    public void ContinueGame() { OnContinue?.Invoke(); }
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
        runTimeData.firstBoot = false;
        DontDestroyOnLoad(gameObject);
        if ((SaveData)SerialisationManager.Load(Application.persistentDataPath + "/Saves" + SaveName + ".save") == null)
            SaveData.current = new SaveData();
        else
            SaveData.current = (SaveData)SerialisationManager.Load(Application.persistentDataPath + "/Saves" + SaveName + ".save");
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
        Instantiate(objectPoolManager, Vector3.zero, Quaternion.identity);
        Instantiate(clientManager, Vector3.zero, Quaternion.identity);
        Instantiate(musicManager, Vector3.zero, Quaternion.identity);
        AudioManager.instance.BindToInitManager();
        TransitionManager.instance.BindToInitManager();
        ObjectPoolManager._instance.BindToInitManager();
        clientManager.GetComponent<IInitialisable>().Init();

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
                BeginNewState(currInitState);


                break;
            case InitStates.GameRunning:
                OnStateChange?.Invoke(newState);
                currGameMode = GameModes.Powercut;
                //LoadingScreen.instance.ToggleScreen(false);
                break;

            case InitStates.LevelLoaded:

                currInitState = newState;
                GameStateManager.instance.BindToInitManager();
                SerialisationManager.Save(SaveName, SaveData.current);
                OnStateChange?.Invoke(currInitState);
                break;

            case InitStates.LevelClear:

                currInitState = newState;
                SerialisationManager.Save(SaveName, SaveData.current);
                OnStateChange?.Invoke(currInitState);

                break;
            case InitStates.ExitLevel:

                LoadingScreen.instance.ToggleScreen(true);
                currInitState = newState;
                SerialisationManager.Save(SaveName, SaveData.current);
                OnStateChange?.Invoke(currInitState);
                break;
            case InitStates.LoadTitleScreen:


                currInitState = newState;
                SerialisationManager.Save(SaveName, SaveData.current);
                OnStateChange?.Invoke(currInitState);

                break;
            case InitStates.TitleScreen:

                LoadingScreen.instance.BeginFade(false);
                currInitState = newState;
                OnStateChange?.Invoke(currInitState);
                currGameMode = GameModes.Menu;

                break;
            case InitStates.Credits:

                LoadingScreen.instance.BeginFade(false);
                currInitState = newState;
                OnStateChange?.Invoke(currInitState);
                currGameMode = GameModes.Menu;

                break;
            case InitStates.LoadMainMenu:


                currInitState = newState;
                OnStateChange?.Invoke(currInitState);

                break;
            case InitStates.MainMenu:

                LoadingScreen.instance.BeginFade(false);
                currInitState = newState;
                currGameMode = GameModes.Menu;
                SerialisationManager.Save(SaveName, SaveData.current);
                OnStateChange?.Invoke(currInitState);
    
                break;
            case InitStates.PlayerSceneLoaded:
                OnStateChange?.Invoke(newState);
                PointerManager.instance.SwitchToCrossHair();
                WeaponManager.instance.BindToInitManager();
                FindObjectOfType<LightManager>().BindToInitManager();
                FindObjectOfType<PlayerBehaviour>().BindToInitManager();
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

    public Record GetRecord() { return record; }
}
