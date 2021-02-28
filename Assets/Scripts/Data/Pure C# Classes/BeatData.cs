using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class BeatData
{

    [SerializeField] private List<ChoiceData> dialogueChoices;
    [SerializeField] private string text;//Body of the beat
    [SerializeField] private int id; //Unique identifier of the beat
    [SerializeField] private int targetBeatID;//if beat chains this the next beat after it
    [SerializeField] private SceneIndex targetScene;//if beat chains this the next beat after it
    [SerializeField] private bool hasScene;//if beat chains this the next beat after it
    [SerializeField] private bool endBeat; //if the beat is the end of the current dialogue sequence
    [SerializeField] private float typeTime;//Unique identifier of the beat
    [SerializeField] private bool isClientBeat; //if the beat is the end of the current dialogue sequence
    [SerializeField] private bool isBeatChain;//if beat leads to another
    public string DisplayText { get { return text; } }
    public int ID { get { return id; } }
    public bool HasScene { get { return hasScene; } }
    public SceneIndex TargetScene { get { return targetScene; } }
    public bool IsEnd { get {return  endBeat; } }
    public int TargetID { get { return targetBeatID; } }
    public bool Chains { get { return isBeatChain; } }

    public float TypeTime { get { return typeTime; } }
    public bool IsClientBeat { get { return isClientBeat; } }

    public List<ChoiceData> GetChoices() { return dialogueChoices; }
}


