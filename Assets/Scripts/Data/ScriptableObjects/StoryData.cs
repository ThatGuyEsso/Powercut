using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Story Data",fileName = "Story.asset")]
//Scriptable object to store story of game
[Serializable]
public class StoryData : ScriptableObject
{
    [SerializeField] private List<BeatData> beats;//List of every story beat

    public BeatData GetBeatById(int id)
    {
        return beats.Find(b => b.ID == id);
    }

#if UNITY_EDITOR
    public const string PathToAsset = "Assets/Data/Story.asset";

    public static StoryData LoadData()
    {
        StoryData data = AssetDatabase.LoadAssetAtPath<StoryData>(PathToAsset);
        if (data == null)
        {
            data = CreateInstance<StoryData>();
            AssetDatabase.CreateAsset(data, PathToAsset);
        }

        return data;
    }


#endif
}
