using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScalingProgressBar : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private float maxValue;
    [SerializeField] private float value;

    public void SetMaxValue(float maxVal) { maxValue = maxVal; }
    public void SetValue(float newVal) { value = newVal; }

    public void UpdateValue(float newVal)
    {
        SetValue(newVal);
    }
}
