using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePromptClearer : MonoBehaviour
{
    DialogueTrigger trigger;


    private void Awake()
    {
        trigger = gameObject.GetComponent<DialogueTrigger>();
        trigger.OnDialogueTriggered += ClearActivePrompts;
    }
    private void ClearActivePrompts()
    {
        trigger.OnDialogueTriggered -= ClearActivePrompts;
        if (TutorialManager.instance != false)
        {
            TutorialManager.instance.ClearActivePrompts();
        }
    }
}
