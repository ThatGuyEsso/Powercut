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
        if (instance == false)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
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
            //Debug.Log("Level Lamp #" + i + " Has been added");
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

    public Transform GetNearestFuseLightFuse(Transform targetObject)
    {
        Transform nearestFuseTransform;

        //Set initial shortest distance (potentially make it random for polish)
        float currShortestDistance = Vector2.Distance(targetObject.position,levelLamps[0].GetLightFuse().transform.position);

        //If the initial lamp is working
        if (levelLamps[0].GetIsLampWorking())
        {
            //the nearest transfrom is its fuse
            nearestFuseTransform = levelLamps[0].GetLightFuse().transform;
        }
        else
        {
            //Else the transform is null
            nearestFuseTransform = null;

        }
        for (int i=0; i < levelLamps.Count; i++)
        {
            //If the current lamp is working, compare distance
            if (levelLamps[i].GetIsLampWorking())
            {
                float distance;
                //If nearest transform equal null we can assume this is the first working light, Hence return this and make this the nearest
                if (nearestFuseTransform == null)
                {
                    currShortestDistance = Vector3.Distance(targetObject.position, levelLamps[i].GetLightFuse().transform.position);
                    nearestFuseTransform = levelLamps[i].GetLightFuse().transform;
                }
                else
                {
                    distance = Vector2.Distance(targetObject.position, levelLamps[i].GetLightFuse().transform.position);
                    if (distance < currShortestDistance)
                    {
                        currShortestDistance = distance;
                        nearestFuseTransform = levelLamps[i].GetLightFuse().transform;
                    }
                }

            }
        

        }

        return nearestFuseTransform;
    }

    
}
