using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class BeatData
{

    [SerializeField] private List<ChoiceData> dialogueChoices;
    [SerializeField] private string text;//Body of the beat
    [SerializeField] private int id; //Unique identifier of the beat
    [SerializeField] private bool endBeat; //if the beat is the end of the current dialogue sequence

    public string DisplayText { get { return text; } }
    public int ID { get { return id; } }
    public bool IsEnd { get {return  endBeat; } }
}


