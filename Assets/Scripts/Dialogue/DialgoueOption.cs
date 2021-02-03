using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DialgoueOption : MonoBehaviour
{
    TextMeshProUGUI displayText;
    int targetBeatIndex;


    public delegate void ChoiceDelegate();
    public event ChoiceDelegate OnDialogueSelected;
    private void Awake()
    {    
            displayText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }


    public void SetUpDialogue(string newDisplayText, int targetBeatIndex)
    {
        displayText.text = newDisplayText;
        this.targetBeatIndex = targetBeatIndex;
    }

    public void SelectDialgoue()
    {
        DialogueManager.instance.DisplayDialogueChoice(targetBeatIndex);
        OnDialogueSelected?.Invoke();
    }
}
