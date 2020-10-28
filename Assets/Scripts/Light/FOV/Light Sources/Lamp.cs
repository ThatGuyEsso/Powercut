using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Lamp : MonoBehaviour
{
    //Light it should reference
    private BaseLampLight lightRef;
    private LightFuse fuseRef;
    public float lightDistanceModifier = 0;//Increases or decreases light distance to allow differenct sized light sources.
    private float currentHealth;
    public bool isLampWorking;

    private void Awake()
    {
        fuseRef = transform.GetComponentInChildren<LightFuse>();
    }

    public void InitialiseLamp(BaseLampLight newLightRef)
    {
        lightRef = newLightRef;
        currentHealth = lightRef.lampSettings.maxLightHealth;
        //Add Modifiers 
        lightRef.AddLightDistanceModifier(lightDistanceModifier);


        //Lamps are stationary so only needs to set origin and aim direction at the beginning
        lightRef.SetOrigin(transform.position);
        lightRef.SetAimDirection(transform.up);
        lightRef.ToggleLight(isLampWorking);
        if (!isLampWorking)
        {
            currentHealth = 0f;
        }
    }

    //Light hurting 
    public void DamageLamp(float Damage)
    {
        //Check if lamp is working before damaging lamp
        if (isLampWorking)
        {
            currentHealth -= Damage;//firstly subtact damage from currentHealth

            //Check wether the lamp is still has health left if not it must not be working
            if (currentHealth <= 0)
            {

      
                isLampWorking = false;
                currentHealth = 0;

            }
            lightRef.ToggleLight(isLampWorking);
        }
    }
    //Light fixing 
    public void FixLamp(float heal)
    {
        //You can only fix broken lamps
        if (!isLampWorking)
        {
            currentHealth += heal;//if it is broken fix lamp

            //Check if lamp is fully healed
            if (currentHealth >= lightRef.lampSettings.maxLightHealth)
            {
                //if so activate lamp and set it to the max health
                isLampWorking = true;
                currentHealth = lightRef.lampSettings.maxLightHealth;
            }

            lightRef.ToggleLight(isLampWorking);
        }
    }

   


    //Setters

    //#End of setters#

        //Getters
    public bool GetIsLampWorking()
    {
        return isLampWorking;
    }

    public bool GetIsFixed()
    {
        return currentHealth >= lightRef.lampSettings.maxLightHealth;
    }

    public LightFuse GetLightFuse()
    {
        return fuseRef;
    }

    // # End of Getters#
}
