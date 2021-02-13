using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Difficulty Settings")]
public class LevelDifficultyData : ScriptableObject
{
    public int maxNumberCrawlers;
    public float targetPercentBrokenLamps;

    public float minLampBreakTime;
    public float maxLampBreakTime;




}
