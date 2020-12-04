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
        currentLevel = SceneIndex.MainMenu;
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
        }
    }

    public void GameInit()
    {
        LoadingScreen.instance.OnFadeComplete += EvaluateFade;
        sceneLoading.Add(SceneManager.LoadSceneAsync((int)(SceneIndex.MainMenu), LoadSceneMode.Additive));
        StartCoroutine(GetGameInitLoadProgress());
    }

    public void ReturnToTitleScreen()
    {

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
  
        Debug.Log("started  fading");
        while (isFading)
        {
            yield return null;
        }
        Debug.Log("Finisied  fading");
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

}
