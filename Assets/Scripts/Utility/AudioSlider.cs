using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public enum AudioSetting
{
    soundEffect,
    UI,
    Music
};
public class AudioSlider : MonoBehaviour
{
    [SerializeField] protected AudioMixerGroup mixerGroup;

    [SerializeField] protected float currentDB;
    [SerializeField] protected Image icon;
    [SerializeField] protected List<Sprite> stateSprites;
    [SerializeField] protected Slider slider;

    [SerializeField] protected AudioSetting audioType;

    protected void OnEnable()
    {
        float value;
        mixerGroup.audioMixer.GetFloat("Volume", out value);

        slider.value=value;
        SetAudioLevel(slider.value);

    }
    
    public void SetAudioLevel(float newValue)
    {
        mixerGroup.audioMixer.SetFloat("Volume", newValue);
        currentDB = newValue;
        EvaluateIcon();

        if(SaveData.current != null)
        {
            switch (audioType)
            {
                case AudioSetting.soundEffect:
                    SaveData.current.soundSettings.soundEffect = currentDB;
                    SerialisationManager.Save(InitStateManager.SaveName, SaveData.current);

                    break;
                case AudioSetting.Music:
                    SaveData.current.soundSettings.music = currentDB;
                    SerialisationManager.Save(InitStateManager.SaveName, SaveData.current);
                    break;

                case AudioSetting.UI:
                    SaveData.current.soundSettings.uiEffect = currentDB;
                    SerialisationManager.Save(InitStateManager.SaveName, SaveData.current);
                    break;
            }
        }
      

    }

    virtual protected void EvaluateIcon()
    {
        float normValue = slider.normalizedValue;

        if (normValue <= 0) icon.sprite = stateSprites[stateSprites.Count - 1];
        else if(normValue>= 1)  icon.sprite = stateSprites[0];
        else if (normValue >0 && normValue <0.5f) icon.sprite = stateSprites[1];
    }




}
