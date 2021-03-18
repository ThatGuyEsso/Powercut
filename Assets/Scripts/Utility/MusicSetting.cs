using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSetting : AudioSlider
{
    protected override void EvaluateIcon()
    {
        float normValue = slider.normalizedValue;

        if (normValue <= 0) icon.sprite = stateSprites[stateSprites.Count - 1];
        else if (normValue > 0) icon.sprite = stateSprites[0];
    }
}
