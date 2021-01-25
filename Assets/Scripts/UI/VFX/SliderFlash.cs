using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthVFX : MonoBehaviour
{
    private ProgressBar healthBarSlider;
    private Color defaultColour;
    private Color flashColour = Color.white;
    [SerializeField] private float flashTIme;
    private void Awake()
    {
        healthBarSlider = gameObject.GetComponent<ProgressBar>();
        defaultColour= healthBarSlider.sliderFill.color;
        healthBarSlider.OnSliderChange += FlashOn;
    }

    private void FlashOn()
    {

    }

    private void FlashOff()
    {

    }
}
