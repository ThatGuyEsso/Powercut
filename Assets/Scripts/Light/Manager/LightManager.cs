using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ChargeStates
{
    Discharging,
    Charging,
    StandBy
}
public class LightManager : MonoBehaviour
{
    //Charge variables
    private float currentCharge;
    private float dischargeRate;
    private float chargeRate;
    //Component cache ref
    private ProgressBar batterySlider;
    private FieldOfView fieldViewCone;
    //Settings
    public LightSettings settings;
    private ChargeStates chargeState;

    private bool isFullyCharged;


    private bool isInitialised;


    public void Init()
    {
        //initial variables
        currentCharge = settings.maxCharge;
        dischargeRate = settings.dischargeRate;
        fieldViewCone = gameObject.GetComponent<FieldOfView>();
        batterySlider = UIManager.instance.batteryDisplay;
        batterySlider.InitSlider(settings.maxCharge);
        isInitialised = true;
        fieldViewCone.ToggleLight(false);
        batterySlider.UpdateSlider(currentCharge);
    }

    public void Update()
    {
        //Switch between charging states
        switch (chargeState)
        {
            case ChargeStates.Charging:
                Charge();
                break;
            case ChargeStates.Discharging:
                Discharge();
                break;
            case ChargeStates.StandBy:
                break;
        }
    }
    //Increase decrease current charge
    private void Discharge()
    {
        if (isInitialised&&fieldViewCone.GetLightIsOn())
        {
            if (dischargeRate > 0)
            {
                dischargeRate -= Time.deltaTime;
            }
            else
            {
                dischargeRate = settings.dischargeRate;
                currentCharge -= settings.disChargeAmount;
                if (currentCharge <= 0)
                {
                    currentCharge = 0;
                }
                batterySlider.UpdateSlider(currentCharge);
            }

            //If there is no charge turn off light if it is on
            if (currentCharge <= 0 && fieldViewCone.GetLightIsOn())
            {
                fieldViewCone.ToggleLight(false);
                SetChargeState(ChargeStates.StandBy);
            }
        }
     
    }

    //Increase current charge
    private void Charge()
    {
      
        if (chargeRate > 0)
        {
            chargeRate -= Time.deltaTime;
        }
        else
        {
            chargeRate = settings.chargeRate;
            currentCharge += settings.rechargeAmount;
            if(currentCharge >= settings.maxCharge)
            {
                currentCharge = settings.maxCharge;
            }
            batterySlider.UpdateSlider(currentCharge);
        }
        //If there is some charge turn on light if it is off
        if (currentCharge>0 && !fieldViewCone.GetLightIsOn())
        {
            fieldViewCone.ToggleLight(true);
          


        }
    }

    //Setters
    public void SetChargeState(ChargeStates newState)
    {
        chargeState = newState;
    }
    //Getters
    public ChargeStates GetChargeState()
    {
        return chargeState;
    }

    public bool GetIsFullyCharged()
    {
        if(currentCharge >= settings.maxCharge)
        {
            isFullyCharged = true;
        }
        else
        {
            isFullyCharged = false;
        }
        return isFullyCharged;
    }

    private void ResetLight()
    {
        currentCharge = settings.maxCharge;
        dischargeRate = settings.dischargeRate;
        batterySlider.UpdateSlider(currentCharge);

        EvaluateGameNewState(GameStateManager.instance.GetCurrentGameState());
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
            case InitStates.PlayerSpawned:
                Init();
            
                break;
            case InitStates.PlayerRespawned:
                ResetLight();

                break;
        }
    }
    private void EvaluateGameNewState(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.LevelClear:
                fieldViewCone.ToggleLight(false);
                break;
            case GameStates.MainPowerOn:
                fieldViewCone.ToggleLight(false);
                break;
            case GameStates.MainPowerOff:
                fieldViewCone.ToggleLight(true);
                break;

        }
    }
}

