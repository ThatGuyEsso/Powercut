using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Lamp : MonoBehaviour, IEnemySpawnable
{
    //Light it should reference
    private BaseLampLight lightRef;
    private LightFuse fuseRef;
    public float lightDistanceModifier = 0;//Increases or decreases light distance to allow differenct sized light sources.
    private float currentHealth;
    public bool isLampWorking;
    private IEnemySpawnable enemySpawner; 

    private void Awake()
    {
        fuseRef = gameObject.GetComponentInChildren<LightFuse>();
        enemySpawner= gameObject.GetComponentInChildren<EnemySpawner>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        //if lamp is broken and player enters stop spawning/due to player light
        if (other.gameObject.CompareTag("Player")){
            if (!isLampWorking)
            {
                enemySpawner.LampInLight();
            }
        }

    }
  
    private void OnTriggerStay2D(Collider2D other)
    {
        //if lamp is broken and player enters stop spawning/due to player light
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isLampWorking)
            {
                enemySpawner.LampInLight();
            }
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //if lamp is broken and player leaves start spawning/due to player leaving with light
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isLampWorking)
            {
                enemySpawner.LampInDarkness();
            }
        }

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
        lightRef.InitLampView();

        if (!isLampWorking)
        {
            currentHealth = 0f;
            enemySpawner.LampInDarkness();
        }
        else
        {
            enemySpawner.LampInLight();
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
                enemySpawner.LampInDarkness();

            }
            lightRef.ToggleLight(isLampWorking);
        }
    }
    public void InstantBreakLamp()
    {
        if (isLampWorking)
        {
            lightRef.ToggleLight(false);
            isLampWorking = false;
            currentHealth = 0;
        }
    }
    public void InstantFixLamp()
    {
   
        lightRef.ToggleLight(true);
        isLampWorking = true;
        currentHealth = lightRef.lampSettings.maxLightHealth;
     
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
                enemySpawner.LampInLight();

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


    void IEnemySpawnable.LampInDarkness()
    {
        
    }

    void IEnemySpawnable.LampInLight()
    {

    }
    // # End of Getters#
}
