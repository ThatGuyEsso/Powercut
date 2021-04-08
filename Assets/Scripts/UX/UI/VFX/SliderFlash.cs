using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderFlash : MonoBehaviour
{
    private ProgressBar healthBarSlider;
    private Color defaultColour;
    [SerializeField] private Color flashColour = Color.white;
    [SerializeField] private float flashTime;
    private void Awake()
    {
        healthBarSlider = gameObject.GetComponent<ProgressBar>();
        healthBarSlider.OnSliderChange += FlashOn;
        defaultColour = healthBarSlider.sliderFill.color;
    }

    private void FlashOn()
    {
        healthBarSlider.sliderFill.color = flashColour;
        Invoke("FlashOff", flashTime);
    }

    private void FlashOff()
    {
        healthBarSlider.sliderFill.color = defaultColour;
    }
}

