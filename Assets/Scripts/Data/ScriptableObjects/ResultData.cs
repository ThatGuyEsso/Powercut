using System;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
[CreateAssetMenu(menuName = "Result Data", fileName = "Assets/Data/Results.asset")]
public class ResultData : ScriptableObject
{

    [SerializeField]
    private List<Result> results;//List of every story beat


    //Find a Result by its name
    public Result GetConsequenceByName(string name)
    {
        return results.Find(c => c.ResultName == name);
    }

    //Reset all results
    public void ResetAllDecisions()
    {
        foreach (Result result in results)
        {
            result.Reset();
        }
    }
}
