﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class MainPowerSwitch : MonoBehaviour, Controls.IInteractionsActions
{
    private Controls input;
    private bool inRange;
    private bool hasActivated;
    
    private void Awake()
    {
        //Inputs
        input = new Controls();
        input.Interactions.SetCallbacks(this);
        input.Enable();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRange = true;

            if(hasActivated == false)
            {
                InGamePrompt.instance.ChangePrompt("[E] Switch off Mains");
                InGamePrompt.instance.ShowPrompt();
            }
           
        }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            inRange = false;
            InGamePrompt.instance.HidePrompt();
        }


    }


    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && inRange)
        {
            //Get current state
            switch (GameStateManager.instance.GetCurrentGameState())
            {
                //If Main power is on
                case GameStates.MainPowerOn:

                    //Switch it off
                    if (hasActivated == false)
                    {
                        GameStateManager.instance.BeginNewGameState(GameStates.MainPowerOff);
                        hasActivated = true;

                    }
                    break;

                case GameStates.TasksCompleted:
                    //When all task completed player should be able to switch mains on again
                    if (LevelLampsManager.instance.GetAllSceneLampsWork())
                    {
                        GameStateManager.instance.BeginNewGameState(GameStates.LevelClear);

                    }
                    break;

            }
        }
    }

}
