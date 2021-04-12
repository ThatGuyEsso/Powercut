using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EventTemplate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;
    private Color displayColor;
    private string eventMessage;
    public float height;
    public RectTransform rt;
    [SerializeField] private Vector2 padding;


    public void SetUpEventTemplate(string message, Color textColor)
    {
        eventMessage = message;
        displayColor = textColor;
        displayText.text = eventMessage;
        displayText.ForceMeshUpdate();
        Vector2 renderBounds = displayText.GetRenderedValues(false);
        displayText.color = displayColor;
        rt.sizeDelta = renderBounds + padding;
    }




}
