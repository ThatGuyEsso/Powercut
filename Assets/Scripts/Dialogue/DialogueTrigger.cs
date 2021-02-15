using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class DialogueTrigger : MonoBehaviour { 
    private Controls input;
    private bool inRange =false;
    private bool hasTriggered=false;

    [SerializeField] private int BeatID;
    [SerializeField] private Speaker speaker;
    private void Awake()
    {
        //Inputs
        input = new Controls();
        input.Interactions.Enable();
        input.Interactions.Interact.performed += TriggerDialogue;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player")&& !hasTriggered)
        {
            inRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && inRange)
        {
            inRange = false;
        }
    }
    private void TriggerDialogue(InputAction.CallbackContext context)
    {
        hasTriggered = true;
        DialogueManager.instance.SetUpNextBeat(BeatID, speaker);
        DialogueManager.instance.ToggleDialogueScreen(true, true);
    }

    private void OnDestroy()
    {
        input.Interactions.Interact.performed -= TriggerDialogue;
    }
}
