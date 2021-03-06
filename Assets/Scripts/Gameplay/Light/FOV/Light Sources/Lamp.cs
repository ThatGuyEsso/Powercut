using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Lamp : MonoBehaviour, IEnemySpawnable,IInitialisable
{
    //Light it should reference
    [SerializeField] protected BaseLampLight lightRef;
    [SerializeField] private LightFuse fuseRef;
    public float lightDistanceModifier = 0;//Increases or decreases light distance to allow differenct sized light sources.
    protected float currentHealth;
    public bool isLampWorking;
    protected IEnemySpawnable enemySpawner;

    protected bool isFlickering = false;
    [SerializeField] private float maxFlickerTime;
    [SerializeField] private float minFlickerTime;
    [SerializeField] private float minFlickerRate;
    [SerializeField] private float maxFlickerRate;
    protected float timeToNextFlciker;
    protected float currFlickerTime;
    [SerializeField] private SpriteRenderer fuseIcon;
    protected AudioSource audioSource;
    virtual public void Init()
    {

        enemySpawner= gameObject.GetComponentInChildren<EnemySpawner>();
        audioSource = gameObject.GetComponent<AudioSource>();
        if (audioSource)
        {
            Sound sound = AudioManager.instance.GetSound("LightFailingSFX");
            audioSource.clip = sound.clip;
            audioSource.volume = sound.volume;
            audioSource.pitch = sound.pitch;
        }
    }

    

   virtual protected void OnTriggerEnter2D(Collider2D other)
    {
        //if lamp is broken and player enters stop spawning/due to player light
        if (other.gameObject.CompareTag("Player")){
            if (!isLampWorking)
            {
                enemySpawner.LampInLight();
            }
        }

    }

    virtual protected void OnTriggerStay2D(Collider2D other)
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

    virtual protected void OnTriggerExit2D(Collider2D other)
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

    virtual protected void Update()
    {
        if (isFlickering)
        {
            if(currFlickerTime<= 0&& isLampWorking==true)
            {
                isFlickering = false;
                InstantBreakLamp();
                audioSource.Stop();
            }
            else
            {

                if (timeToNextFlciker <= 0)
                {
                    if (lightRef.GetLightIsOn()) lightRef.ToggleLight(false);
                    else lightRef.ToggleLight(true);
                    timeToNextFlciker = Random.Range(minFlickerRate,maxFlickerRate);
                    
                }
                else
                {
                    timeToNextFlciker -= Time.deltaTime;

                }
                currFlickerTime -= Time.deltaTime;
            }

        }
    }

    virtual public void InitialiseLamp(BaseLampLight newLightRef)
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
        enemySpawner.SetUp();

        if (!isLampWorking)
        {
            currentHealth = 0f;
            enemySpawner.LampInDarkness();
            fuseIcon.color = Color.red;
        }
        else
        {
            enemySpawner.LampInLight();
            fuseIcon.color = Color.white;
        }
    }

    //Light hurting 
    virtual public void DamageLamp(float Damage)
    {
        //Check if lamp is working before damaging lamp
        if (isLampWorking)
        {
            currentHealth -= Damage;//firstly subtact damage from currentHealth

            //Check wether the lamp is still has health left if not it must not be working
            if (currentHealth <= 0)
            {
                LevelLampsManager.instance.DetermineShouldBreakLight();
                fuseIcon.color = Color.red;
                isLampWorking = false;
                currentHealth = 0;
                enemySpawner.LampInDarkness();

            }
            lightRef.ToggleLight(isLampWorking);
        }
    }
    virtual public void InstantBreakLamp()
    {
        if (isLampWorking)
        {
            LevelLampsManager.instance.DetermineShouldBreakLight();
            fuseIcon.color = Color.red;
            lightRef.ToggleLight(false);
            isLampWorking = false;
            currentHealth = 0;
            enemySpawner.Spawn();
        }
    }
    virtual public void InstantFixLamp()
    {
        
        lightRef.ToggleLight(true);
        isLampWorking = true;
        currentHealth = lightRef.lampSettings.maxLightHealth;
        if (fuseIcon)
            fuseIcon.color = Color.white;
        else
            Debug.Log("No Icon");
             Debug.Log(gameObject);

    }
    //Light fixing 
    virtual public void FixLamp(float heal)
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
                fuseIcon.color = Color.white;
            }

            lightRef.ToggleLight(isLampWorking);
        }
    }




    //Setters

    //#End of setters#

    //Getters
    virtual public bool GetIsLampWorking()
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
    void IEnemySpawnable.Spawn()
    {

    }
    // # End of Getters#


    private void OnDestroy()
    {
        if(lightRef!=false)
        Destroy(lightRef.gameObject);
    }

    public void SetUp()
    {
        throw new System.NotImplementedException();
    }

    virtual public void BeginLampFlicker()
    {
        currFlickerTime = Random.Range(minFlickerTime, maxFlickerTime);
        isFlickering = true;
        audioSource.Play();
    }
}
