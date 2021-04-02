using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject continueButtons;
    [SerializeField] private GameObject defaultButtons;
    private void Awake()
    {

        PointerManager.instance.SwitchToPointer();
        if (SaveData.current.lastSession.isNewSave == false)
        {
            defaultButtons.SetActive(false);
            continueButtons.SetActive(true);
        }
    }
    public void Play()
    {
        //SaveData.current = new SaveData();
        MusicManager.instance.BeginFadeOut();
        TransitionManager.instance.StartLevel(SceneIndex.Tutorial);
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");
        SaveData.current.ClearSave();
        SaveData.current.lastSession.isNewSave = false;
        SerialisationManager.Save(InitStateManager.SaveName, SaveData.current);


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

    public void Continue()
    {
        SaveData.current = (SaveData)SerialisationManager.Load(Application.persistentDataPath + "/Saves" + InitStateManager.SaveName + ".save");
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");

        InitStateManager.instance.ContinueGame();
        if (SaveData.current.lastSession.lastLevel!= SceneIndex.TabletMenu)
            TransitionManager.instance.StartLevel(SaveData.current.lastSession.lastLevel);
        else
            InitStateManager.instance.BeginNewState(InitStates.LoadMainMenu);
    }
    public void Quit()
    {
        AudioManager.instance.PlayAtRandomPitch("ClickSFX");
        SerialisationManager.Save(InitStateManager.SaveName, SaveData.current);
        Application.Quit();
    }
}
