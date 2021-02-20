using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public static PauseScreen instance;
    public GameObject pauseScreen;

    public delegate void PauseDelegate();
    public event PauseDelegate OnPause;
    public event PauseDelegate OnResume;
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

    public void Pause()
    {
       
        if (GameStateManager.instance != false)
        {
            pauseScreen.SetActive(true);
            GameStateManager.instance.PauseGame();
            OnPause?.Invoke();
        }
    }
    public void Resume()
    {

        if (GameStateManager.instance != false)
        {
            AudioManager.instance.PlayAtRandomPitch("ClickSFX");
            pauseScreen.SetActive(false);
            GameStateManager.instance.ResumeGame();
            OnResume?.Invoke();
        }
    }
    public void ReturnToTitleScreen()
    {
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");
        GameStateManager.instance.ResumeGame();
        InitStateManager.instance.BeginNewState(InitStates.LoadTitleScreen);
      
    }


    public void Quit()
    {
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");
        Application.Quit();
    }
}
