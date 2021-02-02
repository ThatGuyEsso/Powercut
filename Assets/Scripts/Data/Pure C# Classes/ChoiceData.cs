using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChoiceData
{
    [SerializeField] private string text; //choice display text
    [SerializeField] private int targetBeatID; //beat choice leads to
    [SerializeField] private string resultName; //choice result to trigger

    public string DisplayText { get { return text; } }
    public int NextID { get { return targetBeatID; } }
    public string ChoiceResult { get { return resultName; } }
    


}
