using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void Play()
    {
        TransitionManager.instance.StartLevel(SceneIndex.Tutorial);
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");
    }

    public void Settings()
    {
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");
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
