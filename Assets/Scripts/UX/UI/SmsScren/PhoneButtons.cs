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


    [SerializeField] private PhoneButtonWidget playWidget, menuWidget;

    [SerializeField] private bool isPlayButtonActive=true, isMenuButtonActive=true;
    private void Awake()
    {
  
        Init();
        
    }
    public void Init()
    {
        if (playButton)
        {
            playButton.enabled = false;
            playWidget.UpdateLabel("Can't do that right now", Color.red);
            playButtonImage.color = inactiveColourButton;
        }
         
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
                        TransitionManager.instance.LoadLevel(targetScene);
                        ClientManager.instance.GetActiveClient().ClearMessage();
                        break;
                    case GameModes.Dialogue:
                        DialogueManager.instance.ToggleDialogueScreen(false, true);
                        break;
                }

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

    public void EnableStartButton()
    {
    
        canStart = true;
        playWidget.ResetLabel();
        playButtonImage.color = activeColour;
        playButton.enabled = true;


    }
}
