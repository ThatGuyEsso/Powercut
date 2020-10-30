using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public ProgressBar batteryDisplay;
    public AmmoDisplay ammoDisplay;
    public ProgressBar healthBarDisplay;
    private void Awake()
    {
        instance = this;
        InitProgressBars();
    }



    public void InitProgressBars()
    {
        batteryDisplay = transform.Find("Battery").GetComponent<ProgressBar>();
        healthBarDisplay = transform.Find("HealthBar").GetComponent<ProgressBar>();
    }
}


 
