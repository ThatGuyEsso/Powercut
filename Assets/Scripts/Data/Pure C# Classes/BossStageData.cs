using System;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class BossStageData
{

    //Stage timing
    [SerializeField] private float timeBtwnPatterns;
    [SerializeField] private float minCycleCooldownTime;
    [SerializeField] private float maxCycleCooldownTime;
    [SerializeField] private BossStage stage;
    //patterns
    [SerializeField] private List<AttackPatternData> attackPatterns;

    public void SetUpData()
    {
        foreach(AttackPatternData pattern in attackPatterns)
        {
            pattern.SetStage = stage;
            pattern.AttackPattern.SetStage(stage);
        }
    }

    public BossStage Stage { get { return stage; } }
    public List<AttackPatternData> AttackPatternData { get { return attackPatterns; } }
    public float MinCycleTime { get { return minCycleCooldownTime; } }
    public float MaxCycleTime { get { return maxCycleCooldownTime; } }

    public float TimeBetweenPatterns { get { return timeBtwnPatterns; } }
    public float RandomCycleCoolDown() { 
        return UnityEngine.Random.Range(minCycleCooldownTime, maxCycleCooldownTime); 
    }
}
