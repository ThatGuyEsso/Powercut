using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialgoueOption : MonoBehaviour
{
    TextMeshProUGUI displayText;
    RectTransform rt;
    [SerializeField] private Vector2 padding;
    [SerializeField] private Vector2 offset;
    [SerializeField] private DialgoueOption prevBubble;
    private Vector2 startPostion;
    int targetBeatIndex;
    ChoiceData choice;


    public delegate void ChoiceDelegate();
    public event ChoiceDelegate OnDialogueSelected;
    private void Awake()
    {    
            displayText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            rt = gameObject.GetComponent<RectTransform>();
            startPostion = rt.position;
    }


    public void SetUpDialogue(string newDisplayText, int targetBeatIndex,ChoiceData newChoice)
    {
        displayText.text = newDisplayText;
        displayText.ForceMeshUpdate();
        Vector2 renderBounds = displayText.GetRenderedValues(false);
        rt.sizeDelta = renderBounds + padding;
        this.targetBeatIndex = targetBeatIndex;
        choice = newChoice;


        if (prevBubble != false)
        {
            RectTransform prevRT = prevBubble.GetComponent<RectTransform>();
            Vector2 offsetFromPrevBubble = (Vector2)prevBubble.transform.position - new Vector2(0.0f, prevRT.rect.height);
            transform.position = offsetFromPrevBubble - new Vector2(0.0f, rt.rect.height / 2) + offset;
        }
    }
    public void SetUpDialogue(string newDisplayText, int targetBeatIndex)
    {
        displayText.text = newDisplayText;
        displayText.ForceMeshUpdate();
        Vector2 renderBounds = displayText.GetRenderedValues(false);
        rt.sizeDelta = renderBounds + padding;
        this.targetBeatIndex = targetBeatIndex;
        choice = null;
        if (prevBubble != false)
        {
            RectTransform prevRT = prevBubble.GetComponent<RectTransform>();
            Vector2 offsetFromPrevBubble = (Vector2)prevBubble.transform.position - new Vector2(0.0f, prevRT.rect.height);
            transform.position = offsetFromPrevBubble - new Vector2(0.0f, rt.rect.height / 2) + offset;
        }
    }

    public void SelectDialgoue()
    {
        AudioManager.instance.PlayRandFromGroup("PhoneButtonSFX");
        if (choice!=null)
        DialogueManager.instance.GetResultByName(choice.ChoiceResult).TriggerResult();
        choice = null;
        DialogueManager.instance.DisplayDialogueChoice(targetBeatIndex);
        OnDialogueSelected?.Invoke();
    }
    public Vector2 GetStartingPosition()
    {
        return startPostion;
    }
}
