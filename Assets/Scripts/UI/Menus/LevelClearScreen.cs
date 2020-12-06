using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LevelClearScreen : MonoBehaviour
{
    [HideInInspector]
    public static LevelClearScreen instance;
    public FadeText textFade;
    public GameObject gameOverScreen, jobCompletedScreen;
    private List<Button> buttons = new List<Button>();
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

        InitButtons();
        textFade = gameObject.GetComponent<FadeText>();
        gameOverScreen.SetActive(false);
        jobCompletedScreen.SetActive(false);


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
        InitStateManager.instance.BeginNewState(InitStates.RespawnPlayer);

    }
    private void ReturnToTitle()
    {
        textFade.OnTextFadeEnd -= ReturnToTitle;
        InitStateManager.instance.BeginNewState(InitStates.ExitLevel);
        TransitionManager.instance.ReturnToTitleScreen();

    }

    public void BeginGameOver()
    {
        gameOverScreen.SetActive(true);
        ToggleButtons(false);
        textFade.OnTextFadeEnd += EnableButtons;

        textFade.BeginTextFadeIn(fadeInRate, fadeInMag);
    }
    public void ClearGameOver()
    {
        ToggleButtons(false);
        textFade.BeginTextFadeOut(fadeOutRate, fadeOutMag);
    }


    public void BeginLevelOver()
    {
        jobCompletedScreen.SetActive(true);
        textFade.BeginTextFadeIn(fadeInRate, fadeInMag);
    }



    //Enables and disables button components
    public void ToggleButtons(bool isOn)
    {
        if(buttons.Count > 0)
        {

            foreach(Button button in buttons)
            {
                button.enabled = isOn;
            }
        }
    }

    private void EnableButtons()
    {
        //Only fire one so unsubscribe when done
        textFade.OnTextFadeEnd -= EnableButtons;
        if (buttons.Count > 0)
        {

            foreach (Button button in buttons)
            {
                button.enabled = true;
            }
        }
    }

    private void DisableButtons()
    {
        if (buttons.Count > 0)
        {

            foreach (Button button in buttons)
            {
                button.enabled = false;
            }
        }
    }

    private void InitButtons()
    {
        Button[] childButtons = gameObject.GetComponentsInChildren<Button>();

        for(int i = 0; i < childButtons.Length; i++)
        {
            buttons.Add(childButtons[i]);
        }
    }
}

