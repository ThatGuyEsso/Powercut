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


}
