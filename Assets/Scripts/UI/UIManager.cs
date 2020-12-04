using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class UIManager : MonoBehaviour, Controls.IUIActions
{
    public static UIManager instance;
    public ProgressBar batteryDisplay;
    public AmmoDisplay ammoDisplay;
    public ProgressBar healthBarDisplay;
    public ActiveGunDisplay gunDisplay;
    public TaskDisplay taskDisplay;
    public GadgetDisplay gadgetDisplay;
    public EventDisplay eventDisplay;
    private Controls input;
    private bool isFading;
    private void Awake()
    {
        if (instance == false)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

    }
    public void Init()
    {

        GetChildReferences();
        input = new Controls();
        input.UI.SetCallbacks(this);
        input.Enable();
    }


    void OnDestroy()
    {
        input.Disable();
    }
    void OnDisable()
    {
        input.Disable();
    }
    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            AudioManager.instance.PlayAtRandomPitch("ClickSFX");
            if (GameStateManager.isGamePaused)
            {
                PauseScreen.instance.Resume();
            }
            else
            {
                PauseScreen.instance.Pause();
            }
        }
    }
    public void GetChildReferences()
    {
        batteryDisplay = transform.Find("Battery").GetComponent<ProgressBar>();
        healthBarDisplay = transform.Find("HealthBar").GetComponent<ProgressBar>();
        gunDisplay = transform.Find("ActiveGunDisplay").GetComponent<ActiveGunDisplay>();
        taskDisplay = transform.Find("TaskDisplay").GetComponent<TaskDisplay>();
        gadgetDisplay = transform.Find("GadgetDisplay").GetComponent<GadgetDisplay>();
        eventDisplay = transform.Find("EventDisplay").GetComponent<EventDisplay>();
    }
    public void BindToInitManager()
    {
        InitStateManager.instance.OnStateChange += EvaluateNewState;
        GameStateManager.instance.OnGameStateChange += EvaluateGameNewState;
    }
    private void EvaluateNewState(InitStates newState)
    {
        switch (newState)
        {
            case InitStates.PlayerDead:

                StartCoroutine(DeathTransition());
                break;
            case InitStates.RespawnPlayer:
                
            
                break;
        }
    }
    private void EvaluateGameNewState(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.LevelClear:
                eventDisplay.CreateEvent("Main Power Switched On", Color.green);
                eventDisplay.CreateEvent("Level Cleared", Color.green);
          
                break;
            case GameStates.MainPowerOff:
                eventDisplay.CreateEvent("Main Power Switched Off", Color.green);

                break;
        }
    }
    private IEnumerator DeathTransition()
    {
        LoadingScreen.instance.OnCurtainCallEnd += ResolveFading;
        LoadingScreen.instance.StartCurtainCall(0.000005f, false);
        isFading = true;
        while (isFading)
        {
            yield return null;
        }
        LoadingScreen.instance.OnCurtainCallEnd -= ResolveFading;
        LevelClearScreen.instance.BeginGameOver();

    }

    private void ResolveFading()
    {
        isFading = false;
    }
}


 
