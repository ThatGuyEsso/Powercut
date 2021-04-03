using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

[CreateAssetMenu(menuName = "Credit Data", fileName = "Assets/Data/Credits.asset")]
public class Credits : ScriptableObject
{
    public List<CreditData> credit = new List<CreditData>();//List of every story beat
    public Color creditColour;
    public Color authorColour;



}
