using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    bool isTriggered =false;

    private void Awake()
    {
        InitStateManager.instance.OnStateChange += EvaluateNewInitState;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")&&!isTriggered)
        {

            CameraManager.instance.SwitchToCutScene();
            isTriggered = true;
        }
    }
    private void EvaluateNewInitState(InitStates newState)
    {
        switch (newState)
        {
            case InitStates.PlayerRespawned:
            isTriggered = false;

            break;
        }
    }
}
