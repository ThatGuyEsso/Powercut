using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    [SerializeField] private Car car;
    [SerializeField] private DialogueTrigger dialogueTrigger;

    private void Awake()
    {
        
        dialogueTrigger.enabled = false;
        GameStateManager.instance.OnGameStateChange += EvaluateNewState;
    }


    private void EvaluateNewState(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.LevelClear:

                car.SetHasActivated(true);
                dialogueTrigger.enabled = true;
                dialogueTrigger.EnableTrigger();
                break;

        }
    }

    private void OnDestroy()
    {
        GameStateManager.instance.OnGameStateChange -= EvaluateNewState;
    }
}
