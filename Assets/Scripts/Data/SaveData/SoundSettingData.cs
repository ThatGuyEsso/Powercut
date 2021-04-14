using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundSettingData
{
    public float soundEffect =0;
    public float uiEffect=0;
    public float music=0;

    public void Reset()
    {
        soundEffect = 0f;
        uiEffect = 0f;
        music = 0f;
    }
}

