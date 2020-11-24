using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EventTemplate : MonoBehaviour
{
    private TextMeshProUGUI displayText;
    private Color displayColor;
    private string eventMessage;
    public float height;
    public RectTransform rt;
    public void Init()
    {
        displayText = gameObject.GetComponent<TextMeshProUGUI>();
        rt = gameObject.GetComponent<RectTransform>();
        height = rt.rect.height;
    }

    public void SetUpEventTemplate(string message, Color textColor)
    {
        eventMessage = message;
        displayColor = textColor;
        displayText.text = eventMessage;
        displayText.color = displayColor;
    }




}
