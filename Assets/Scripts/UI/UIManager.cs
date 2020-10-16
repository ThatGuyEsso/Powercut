using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public ProgressBar batteryDisplay;
    private void Awake()
    {
        instance = this;
        InitProgressBars();
    }



    public void InitProgressBars()
    {
        batteryDisplay = transform.Find("Battery").GetComponent<ProgressBar>();
    }
}


 
