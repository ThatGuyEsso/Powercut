using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseScreen : MonoBehaviour
{
    public static PauseScreen instance;
    public GameObject pauseScreen;
    public TextMeshProUGUI settingsText;
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
            InitStateManager.currGameMode = GameModes.Menu;
        
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
            InitStateManager.currGameMode = GameModes.Powercut;
          
        }
    }
    public void ToggleSettings()
    {
        if (SettingsMenu.instance.IsVisible())
        {
            SettingsMenu.instance.ToggleSettings(false, false);
            settingsText.text = "Settings";
            settingsText.color = Color.white;
        }
        else
        {
            SettingsMenu.instance.ToggleSettings(true, false);
      
            settingsText.text = "Hide";
            settingsText.color = Color.yellow;
        }

    }
    public void ReturnToTitleScreen()
    {
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");
        GameStateManager.instance.ResumeGame();
        pauseScreen.SetActive(false);

        InitStateManager.instance.BeginNewState(InitStates.LoadTitleScreen);
      
    }


    public void Quit()
    {
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");
        Application.Quit();
    }
}
