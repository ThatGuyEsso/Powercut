using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public static PauseScreen instance;
    public GameObject pauseScreen;

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
        }
    }
    public void Resume()
    {

        if (GameStateManager.instance != false)
        {
            AudioManager.instance.PlayAtRandomPitch("ClickSFX");
            pauseScreen.SetActive(false);
            GameStateManager.instance.ResumeGame();
        }
    }
    public void ReturnToTitleScreen()
    {
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");
    }


    public void Quit()
    {
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");
        Application.Quit();
    }
}
