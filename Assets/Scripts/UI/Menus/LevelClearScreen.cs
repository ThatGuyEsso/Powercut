using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelClearScreen : MonoBehaviour
{
    [HideInInspector]
    public static LevelClearScreen instance;
    public FadeText textFade;
    public GameObject gameOverScreen;
    public float fadeInRate, fadeInMag, fadeOutRate, fadeOutMag;

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

        textFade = gameObject.GetComponent<FadeText>();
        gameOverScreen.SetActive(false);


    }
    public void BeginRetry()
    {
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");
        textFade.OnTextFadeEnd += Retry;
        ClearGameOver();
    }

    public void BeginQuitApp()
    {
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");
        textFade.OnTextFadeEnd += Quit;
        ClearGameOver();
    }
    public void BeginReturnToTitle()
    {
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");
        textFade.OnTextFadeEnd += ReturnToTitle;
        ClearGameOver();
    }
    private void Quit()
    {
        textFade.OnTextFadeEnd -= Quit;
        InitStateManager.instance.BeginNewState(InitStates.ExitLevel);
        Application.Quit();
    }
    private void Retry()
    {
        textFade.OnTextFadeEnd -= Retry;
     

    }
    private void ReturnToTitle()
    {
        textFade.OnTextFadeEnd -= ReturnToTitle;
        InitStateManager.instance.BeginNewState(InitStates.ExitLevel);

    }

    public void BeginGameOver()
    {
        gameOverScreen.SetActive(true);
        textFade.BeginTextFadeIn(fadeInRate, fadeInMag);
    }
    public void ClearGameOver()
    {
        textFade.BeginTextFadeOut(fadeOutRate, fadeOutMag);
    }

}

