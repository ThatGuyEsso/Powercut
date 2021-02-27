using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialgoueOption : MonoBehaviour
{
    TextMeshProUGUI displayText;
    int targetBeatIndex;
    ChoiceData choice;

    public delegate void ChoiceDelegate();
    public event ChoiceDelegate OnDialogueSelected;
    private void Awake()
    {    
            displayText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }


    public void SetUpDialogue(string newDisplayText, int targetBeatIndex,ChoiceData newChoice)
    {
        displayText.text = newDisplayText;
        this.targetBeatIndex = targetBeatIndex;
        choice = newChoice;
    }
    public void SetUpDialogue(string newDisplayText, int targetBeatIndex)
    {
        displayText.text = newDisplayText;
        this.targetBeatIndex = targetBeatIndex;
        choice = null;
     
    }

    public void SelectDialgoue()
    {
        if(choice!=null)
        DialogueManager.instance.GetResultByName(choice.ChoiceResult).TriggerResult();
        choice = null;
        DialogueManager.instance.DisplayDialogueChoice(targetBeatIndex);
        OnDialogueSelected?.Invoke();
    }
}
