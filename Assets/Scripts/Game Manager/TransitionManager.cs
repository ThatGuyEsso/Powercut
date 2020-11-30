using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransitionManager : MonoBehaviour
{
    public static TransitionManager instance;


    private SceneIndex currentGameScene;
    private List<AsyncOperation> sceneLoading = new List<AsyncOperation>();
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
        currentGameScene = SceneIndex.MainMenu;
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
                Debug.Log("Transition manager is bound");
                break;
        }
    }

    public void GameInit()
    {
        sceneLoading.Add(SceneManager.LoadSceneAsync((int)(SceneIndex.MainMenu), LoadSceneMode.Additive));
        StartCoroutine(GetGameInitLoadProgress());
    }
    private void LoadDungeonLevel(SceneIndex scene)
    {
        //LoadingScreen.instance.ToggleScreen(true);
        sceneLoading.Add(SceneManager.UnloadSceneAsync((int)currentGameScene));
        sceneLoading.Add(SceneManager.LoadSceneAsync((int)(scene),LoadSceneMode.Additive));
      
        currentGameScene = scene;
        StartCoroutine( PlaySceneLoadProgress());
    }
    public IEnumerator LoadPlayerScene()
    {
      
     
        sceneLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndex.PlayerScene, LoadSceneMode.Additive));

        for (int i = 0; i < sceneLoading.Count; i++)
        {
            while (!sceneLoading[i].isDone)
            {
                yield return null;
            }
        }

        sceneLoading.Clear();
        InitStateManager.instance.BeginNewState(InitStates.SpawnPlayer);
    }


    public IEnumerator GetSceneLoadProgress()
    {
        for (int i = 0; i < sceneLoading.Count; i++)
        {
            while (!sceneLoading[i].isDone)
            {
                yield return null;
            }
        }
      
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
        //LoadingScreen.instance.ToggleScreen(false);
        sceneLoading.Clear();


    }
    public IEnumerator PlaySceneLoadProgress()
    {
        for (int i = 0; i < sceneLoading.Count; i++)
        {
            while (!sceneLoading[i].isDone)
            {
                yield return null;
            }
        }

        sceneLoading.Clear();
  
    }
}
