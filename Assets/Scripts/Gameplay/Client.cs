using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Client
{
    [SerializeField] private string clientName;
    [SerializeField] private string UID;
    private int nextDialogue;
    public bool hasMessage;
    public bool unlocked;

    [SerializeField] private Sprite clientSprite;
    [SerializeField] private List<SceneIndex> levelTriggers;
    [SerializeField] private List<int> beats;

    [SerializeField] private Dictionary<SceneIndex, int> dialgoueDic = new Dictionary<SceneIndex, int>();

    public int DialogueBeat { get { return nextDialogue; } }
    public string ClientID { get { return UID; } }
    public Sprite ClientImage { get { return clientSprite; } }


    public List<SceneIndex> LevelTriggers { get { return levelTriggers; } }
    public List<int> Beats { get { return beats; } }

    public void BindToResult(SceneIndex trigger, int beatID)
    {
        dialgoueDic.Add(trigger, beatID);
    }

    public bool PointToNewBeat(SceneIndex trigger)
    {
        int id;
        if (dialgoueDic.TryGetValue(trigger, out id))
        {
            nextDialogue = id;
            hasMessage = true;
            return true;
        }
        else
        {
            return false;
        }
    }
    public void ClearMessage()
    {
        hasMessage = false;
      
    }
    
}
