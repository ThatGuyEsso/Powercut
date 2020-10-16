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
    private DynamicLight lightCone;
    //Settings
    public LightSettings settings;
    private ChargeStates chargeState;

    private bool isFullyCharged;


 


    private void Awake()
    {
        //initial variables
        currentCharge = settings.maxCharge;
        dischargeRate = settings.dischargeRate;
        lightCone = gameObject.GetComponent<DynamicLight>();
    }

    private void Start()
    {
        batterySlider = UIManager.instance.batteryDisplay;
        batterySlider.InitSlider(settings.maxCharge);
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
        if (currentCharge <= 0 && lightCone.GetLightIsOn())
        {
            lightCone.ToggleLight(false);
            SetChargeState(ChargeStates.StandBy);
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
        if (currentCharge>0 && !lightCone.GetLightIsOn())
        {
            lightCone.ToggleLight(true);
          


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
}

