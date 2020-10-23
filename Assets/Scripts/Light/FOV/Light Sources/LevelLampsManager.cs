using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLampsManager : MonoBehaviour
{
    public static LevelLampsManager instance;//Sets up for singleton class (one per level)
    private List<Lamp> levelLamps = new List<Lamp>();
    public GameObject lampLightPrefab;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GetAllSceneLamps();
        SetUpSceneLamps();
    }

    private void GetAllSceneLamps()
    {
       Lamp[] lamps  = FindObjectsOfType<Lamp>();//Get all scene lamos

        //Assign each lamp in scene to levelLamps list
        for(int i = 0;i < lamps.Length; i++)
        {
            levelLamps.Add(lamps[i]);
            Debug.Log("Level Lamp #" + i + " Has been added");
        }
    }

    private void SetUpSceneLamps()
    {
        foreach(Lamp lamp in levelLamps)
        {
            BaseLampLight lampLight = Instantiate(lampLightPrefab, Vector3.zero, Quaternion.identity).GetComponent<BaseLampLight>();
            lamp.InitialiseLamp(lampLight);
        }
    }
}
