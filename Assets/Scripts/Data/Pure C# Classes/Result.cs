using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class Result
{
    //name of result so it can be identfied more easily
    [SerializeField] private string resultName;

    //Shows wether the consequence has been triggered yet
    [SerializeField] private bool hasTriggered = false;

    //Result delegate. If this delegate corrosponding delegate is triggered
    public event ResultDelegate OnTriggerResult;
    public delegate void ResultDelegate();
    public void TriggerResult()
    {
        hasTriggered = true;
        OnTriggerResult?.Invoke();
    }

    //reset decison when game is restarted
    public void Reset()
    {
        hasTriggered = false;
    }

}
