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
    public TaskDisplay taskDisplay;
    public GadgetDisplay gadgetDisplay;
    public EventDisplay eventDisplay;
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
        taskDisplay = transform.Find("TaskDisplay").GetComponent<TaskDisplay>();
        gadgetDisplay = transform.Find("GadgetDisplay").GetComponent<GadgetDisplay>();
        eventDisplay = transform.Find("EventDisplay").GetComponent<EventDisplay>();
    }
}


 
