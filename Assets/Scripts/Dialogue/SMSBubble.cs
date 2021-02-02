using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SMSBubble : MonoBehaviour
{
    [SerializeField] private Image bubbleImage;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private RectTransform rt;

    [SerializeField] private string displayText;
    [Tooltip("Filler to the background Image size")]
    [SerializeField] private Vector2 padding; 
    private void Start()
    {
        SetUp(displayText);
    }



    public void SetUp(string text)
    {
        dialogueText.text = text;
        dialogueText.ForceMeshUpdate();
        Vector2 renderBounds= dialogueText.GetRenderedValues(false);
        rt.sizeDelta = renderBounds + padding;
    }
}
