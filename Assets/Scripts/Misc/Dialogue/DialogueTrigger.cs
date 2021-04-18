using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class DialogueTrigger : MonoBehaviour { 
    private Controls input;
    private bool inRange =false;
    private bool hasTriggered=false;
  
    [SerializeField] private Sprite clientPortrait;
    [SerializeField] private int BeatID;
    [SerializeField] private Speaker speaker;
    [SerializeField] private bool isActive = true;
    [SerializeField] private string prompt;

    public delegate void TriggerDelegate();
    public event TriggerDelegate OnDialogueTriggered;
    private void Awake()
    {
        //Inputs
        input = new Controls();
        input.Interactions.Enable();
        input.Interactions.Interact.performed += TriggerDialogue;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")&& !hasTriggered&& isActive)
        {
            inRange = true;
            InGamePrompt.instance.ChangePrompt(prompt);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && inRange)
        {
            InGamePrompt.instance.HidePrompt();
            inRange = false;
        }
    }
    private void TriggerDialogue(InputAction.CallbackContext context)
    {
        if (!hasTriggered && inRange)
        {
            OnDialogueTriggered?.Invoke();
            hasTriggered = true;
            InGamePrompt.instance.HidePrompt();
            input.Interactions.Interact.performed -= TriggerDialogue;
            DialogueManager.instance.SetUpNextBeat(BeatID, speaker);
            DialogueManager.currentClientPortrait = clientPortrait;
            DialogueManager.instance.ToggleDialogueScreen(true, true);
            InGamePrompt.instance.SetColor(Color.white);
            InGamePrompt.instance.ChangePrompt(prompt);

            if (LevelClearScreen.instance)
            {
                LevelClearScreen.instance.ClearLevelOver();
            }
         
        }

    }
    public void EnableTrigger()
    {
        isActive = true;
    }
    private void OnDestroy()
    {
        input.Interactions.Interact.performed -= TriggerDialogue;
    }
}
