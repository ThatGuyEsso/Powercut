using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PhoneButtons : MonoBehaviour
{
   //colours of button

    [SerializeField] private Button playButton;
    [SerializeField] private Image playButtonImage;
    [SerializeField] private Color inactiveColourButton;
    [SerializeField] private Color activeColour;
    SceneIndex targetScene;
    bool canStart =false;

    private void Awake()
    {
  
        Init();
        
    }
    public void Init()
    {
        playButton.enabled = false;
        playButtonImage.color = inactiveColourButton;
    }
    public void StartLevel()
    {
        if (canStart)
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
    public void TitleScreen()
    {
        InitStateManager.instance.BeginNewState(InitStates.LoadTitleScreen);
    }

    public void ReturnToMenus()
    {
        InitStateManager.instance.BeginNewState(InitStates.LoadTitleScreen);
    }


    public void EnableStartButton(SceneIndex scene)
    {
        targetScene = scene;
        canStart = true;
        playButtonImage.color = activeColour;
        playButton.enabled = true;
    }
}
