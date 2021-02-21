using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PhoneButtonWidget : MonoBehaviour
{
 
    [SerializeField] private RectTransform label;
    [SerializeField] private TextMeshProUGUI labelText;
    [SerializeField] private string defaultText;
    [SerializeField] Color defaultColour;
    [SerializeField] Color unavailableColour;
    [SerializeField] Vector2 padding;

    private void Awake()
    {
      
        HideLabel();
    }
    public void ShowLabel()
    {
        
        if (!label.gameObject.activeSelf) label.gameObject.SetActive(true);
        UpdateLabelDimensions();
    }

    public void HideLabel()
    {
        if (label.gameObject.activeSelf) label.gameObject.SetActive(false);
    }

    public void UpdateLabel(string text, Color color)
    {
        labelText.text = text;
        labelText.color = color;
        labelText.ForceMeshUpdate();
        Vector2 renderBounds = labelText.GetRenderedValues(false);
        label.sizeDelta = renderBounds + padding;
    }

    private void UpdateLabelDimensions()
    {
        labelText.ForceMeshUpdate();
        Vector2 renderBounds = labelText.GetRenderedValues(false);
        label.sizeDelta = renderBounds + padding;
    }

    public void ResetLabel()
    {
        UpdateLabel(defaultText, defaultColour);
    }
}
