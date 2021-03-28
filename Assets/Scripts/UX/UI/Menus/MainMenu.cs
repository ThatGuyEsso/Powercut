using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
   

    private void Awake()
    {

         PointerManager.instance.SwitchToPointer();
    }
    public void Play()
    {
        MusicManager.instance.BeginFadeOut();
        TransitionManager.instance.StartLevel(SceneIndex.IceRink);
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");
    }

    public void Settings()
    {
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");
        SettingsMenu.instance.ToggleSettings(true, true);
    }
    public void Credits()
    {
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");
    }
    public void Quit()
    {
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");
        Application.Quit();
    }
}
