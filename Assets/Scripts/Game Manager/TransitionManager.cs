using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;


    private SceneIndex currentLevel;
    private List<AsyncOperation> sceneLoading = new List<AsyncOperation>();
    private bool isLoading = false;
    private bool isFading =false;
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
        currentLevel = SceneIndex.TitleScreen;
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
                GameInit();

                break;
            case InitStates.LoadTitleScreen:
                ReturnToTitleScreen();
                break;
            case InitStates.LoadMainMenu:
                StartCoroutine(LoadTabletMenu());
                break;
        }
    }

    public void GameInit()
    {
        LoadingScreen.instance.OnFadeComplete += EvaluateFade;
        sceneLoading.Add(SceneManager.LoadSceneAsync((int)(SceneIndex.TitleScreen), LoadSceneMode.Additive));
        StartCoroutine(GetGameInitLoadProgress());
    }

    public void ReturnToTitleScreen()
    {
        StartCoroutine(LoadTitle());
    }



    public void LoadLevel(SceneIndex newScene,bool shouldFade)
    {
        if (shouldFade) LoadingScreen.instance.BeginFade(true);
        sceneLoading.Add(SceneManager.UnloadSceneAsync((int)currentLevel));
        sceneLoading.Add(SceneManager.LoadSceneAsync((int)newScene, LoadSceneMode.Additive));
        currentLevel = newScene;
        StartCoroutine(GetSceneLoadProgress());
    }

    public void StartLevel(SceneIndex newLevel)
    {
        StartCoroutine(BeginGameLoad(newLevel));
   
    }


    private IEnumerator BeginGameLoad(SceneIndex newLevel)
    {
        isLoading = true;
        LoadingScreen.instance.BeginFade(true);
        isFading = true;

        //wait till fade end before initialising level
  
        while (isFading)
        {
            yield return null;
        }

        //Unload currentlevel (e.g. mainMenu)
        AsyncOperation sceneUnload = (SceneManager.UnloadSceneAsync((int)currentLevel));
        while (!sceneUnload.isDone)
        {
            yield return null;
        }
        //Load in new level
        AsyncOperation sceneLoad = (SceneManager.LoadSceneAsync((int)newLevel, LoadSceneMode.Additive));
        while (!sceneLoad.isDone)
        {
            yield return null;
        }
        currentLevel = newLevel; // once done current level is new level
        InitStateManager.instance.BeginNewState(InitStates.LevelLoaded);
        //loadUi
        sceneLoad = SceneManager.LoadSceneAsync((int)SceneIndex.UIscene, LoadSceneMode.Additive);
        while (!sceneLoad.isDone)
        {
            yield return null;
        }
        InitStateManager.instance.BeginNewState(InitStates.UISceneLoaded);
        //load in player scene
        sceneLoad = (SceneManager.LoadSceneAsync((int)SceneIndex.PlayerScene, LoadSceneMode.Additive));
        while (!sceneLoad.isDone)
        {
            yield return null;
        }
        InitStateManager.instance.BeginNewState(InitStates.PlayerSceneLoaded);

        isLoading = false;

    }

    private IEnumerator LoadTitle()
    {
        isLoading = true;
        LoadingScreen.instance.BeginFade(true);
        isFading = true;

        //wait till fade end before initialising level

        while (isFading)
        {
            yield return null;
        }

        //get all currently loaded scenes
        Scene[] loadedScenes = GetAllActiveScenes();

        //add and unload operations
        foreach (Scene scene in loadedScenes)
        {
            if(scene.buildIndex != (int)SceneIndex.ManagerScene)
            sceneLoading.Add(SceneManager.UnloadSceneAsync(scene));
        }

        //wait until every scene has unloaded
        for (int i = 0; i < sceneLoading.Count; i++)
        {
            while (!sceneLoading[i].isDone)
            {
                yield return null;
            }
        }
        //clear scens loading
        sceneLoading.Clear();

        //begin loading title screen
        AsyncOperation titleScreenScene = SceneManager.LoadSceneAsync((int)SceneIndex.TitleScreen, LoadSceneMode.Additive);

        while (!titleScreenScene.isDone)
        {
            yield return null;

        }
        InitStateManager.instance.BeginNewState(InitStates.TitleScreen);
        currentLevel = SceneIndex.TitleScreen;

    }


    private IEnumerator LoadTabletMenu()
    {
        isLoading = true;
        LoadingScreen.instance.BeginFade(true);
        isFading = true;

        //wait till fade end before initialising level

        while (isFading)
        {
            yield return null;
        }

        //get all currently loaded scenes
        Scene[] loadedScenes = GetAllActiveScenes();

        //add and unload operations
        foreach (Scene scene in loadedScenes)
        {
            if (scene.buildIndex != (int)SceneIndex.ManagerScene)
                sceneLoading.Add(SceneManager.UnloadSceneAsync(scene));
        }

        //wait until every scene has unloaded
        for (int i = 0; i < sceneLoading.Count; i++)
        {
            while (!sceneLoading[i].isDone)
            {
                yield return null;
            }
        }
        //clear scens loading
        sceneLoading.Clear();

        //begin loading title screen
        AsyncOperation menu = SceneManager.LoadSceneAsync((int)SceneIndex.TabletMenu, LoadSceneMode.Additive);

        while (!menu.isDone)
        {
            yield return null;

        }
        InitStateManager.instance.BeginNewState(InitStates.MainMenu);
        currentLevel = SceneIndex.TabletMenu;
    
    }
    public IEnumerator GetSceneLoadProgress()
    {
        isLoading = true;
        for (int i = 0; i < sceneLoading.Count; i++)
        {
            while (!sceneLoading[i].isDone)
            {
                yield return null;
            }
        }
        isLoading = false;
        sceneLoading.Clear();


    }
    public IEnumerator GetGameInitLoadProgress()
    {
        for (int i = 0; i < sceneLoading.Count; i++)
        {
            while (!sceneLoading[i].isDone)
            {
                yield return null;
            }
        }
        LoadingScreen.instance.BeginFade(false);
        sceneLoading.Clear();


    }


    public void EvaluateFade()
    {
        isFading = false;

    }

    private Scene[] GetAllActiveScenes()
    {
        //Get all number of scenes loaded
        int countLoaded = SceneManager.sceneCount;

        //create array of respective size
        Scene[] loadedScenes = new Scene[countLoaded];

        //get all loaded scnes
        for (int i = 0; i < countLoaded; i++)
        {
            loadedScenes[i] = (SceneManager.GetSceneAt(i));
        }
    //retun loaded scenes
        return loadedScenes;
    }

}
