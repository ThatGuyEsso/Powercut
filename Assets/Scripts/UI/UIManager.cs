using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public ProgressBar batteryDisplay;
    public AmmoDisplay ammoDisplay;
    public ProgressBar healthBarDisplay;
    public ActiveGunDisplay gunDisplay;
    private void Awake()
    {
        instance = this;
        GetChildReferences();
    }



    public void GetChildReferences()
    {
        batteryDisplay = transform.Find("Battery").GetComponent<ProgressBar>();
        healthBarDisplay = transform.Find("HealthBar").GetComponent<ProgressBar>();
        gunDisplay = transform.Find("ActiveGunDisplay").GetComponent<ActiveGunDisplay>();
    }
}


 
