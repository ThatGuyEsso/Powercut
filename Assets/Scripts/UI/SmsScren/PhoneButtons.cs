using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class PhoneButtons : MonoBehaviour
{
   //colours of button

    [SerializeField] private Button playButton;
    [SerializeField] private Image playButtonImage;
    [SerializeField] private Color inactiveColourButton;
    [SerializeField] private Color activeColour;
    SceneIndex targetScene;
    bool canStart =false;


    [SerializeField] private PhoneButtonWidget playWidget, menuWidget, titleWidget;

    [SerializeField] private bool isTitleButtonActive =true, isPlayButtonActive=true, isMenuButtonActive=true;
    private void Awake()
    {
  
        Init();
        
    }
    public void Init()
    {
        playButton.enabled = false;
        playWidget.UpdateLabel("Can't do that right now", Color.red);
        playButtonImage.color = inactiveColourButton;
    }
    public void StartLevel()
    {
        if (canStart)
        {
            playWidget.ResetLabel();
            if (isPlayButtonActive)
            {

                switch (InitStateManager.currGameMode)
                {
                    case GameModes.Menu:
                        TransitionManager.instance.LoadLevel(targetScene, true);
                        break;
                    case GameModes.Dialogue:
                        DialogueManager.instance.ToggleDialogueScreen(false, true);
                        break;
                }

            }


        }


    }

    public void TitleScreen()
    {
        if (isTitleButtonActive)
        {

            InitStateManager.instance.BeginNewState(InitStates.LoadTitleScreen);
        }
        else
        {
            if (titleWidget.gameObject.activeSelf)
            {
                titleWidget.UpdateLabel("can't do that right now", Color.red);
            }
        }
    }

    public void ReturnToMenus()
    {
        if (isMenuButtonActive)
        {
            InitStateManager.instance.BeginNewState(InitStates.LoadTitleScreen);

        }
        else
        {
            if (menuWidget.gameObject.activeSelf)
            {
                menuWidget.UpdateLabel("Can't do that right now", Color.red);
            }
        }
    }


    public void EnableStartButton(SceneIndex scene)
    {
        targetScene = scene;
        canStart = true;
        playWidget.ResetLabel();
        playButtonImage.color = activeColour;
        playButton.enabled = true;
  

    }
}
