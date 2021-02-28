using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLampsManager : MonoBehaviour, IInitialisable
{
    [SerializeField] private LevelDifficultyData difficultySettings;
    public static LevelLampsManager instance;//Sets up for singleton class (one per level)
    private List<Lamp> levelLamps = new List<Lamp>();
    public GameObject lampLightPrefab;

    //Disable lights mechanism (should be moved into a scriptable object to allow for difficulty easy difficulty scaling)

    private float targetLightWorkingPercent; //How many lights that can be broken before we stop breaking light.
    private bool shouldBreakLights;
    private float currentTimeBeforeLightBreak;

    GameStates currentGameState;

    public delegate void LampBrokeDelegate();
    public event LampBrokeDelegate OnLampBroke;
    private void Init()
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

        GetNewBreakLampTime();
        BindToInitManager();
        GetAllSceneLamps();
        SetUpSceneLamps();
        targetLightWorkingPercent = 1 - difficultySettings.targetPercentBrokenLamps;
    }



    private void Update()
    {
        switch (currentGameState)
        {
            case GameStates.MainPowerOff:
            if (shouldBreakLights)
            {
                if(currentTimeBeforeLightBreak <= 0)
                {
                    BreakRandomLamp();

                    GetNewBreakLampTime();
                }
                else
                {
                    currentTimeBeforeLightBreak -= Time.deltaTime;
                }
            }
            break;
           

        }
    }
    //Gets all lamps in scene and stores then in list
    public void GetAllSceneLamps()
    {
        Lamp[] lamps = FindObjectsOfType<Lamp>();//Get all scene lamos

        //Assign each lamp in scene to levelLamps list
        for (int i = 0; i < lamps.Length; i++)
        {
            levelLamps.Add(lamps[i]);
            //Debug.Log("Level Lamp #" + i + " Has been added");
        }
    }

    public void FixAllSceneLamps() {
        //if any lamp is broken return false
        foreach (Lamp lamp in levelLamps)
        {
            lamp.InstantFixLamp();
        }
    }
    private void SetUpSceneLamps()
    {
        //Spawns in a light source for each scene lamp. (Should be updated to object pooling)
        foreach (Lamp lamp in levelLamps)
        {
            BaseLampLight lampLight = Instantiate(lampLightPrefab, Vector3.zero, Quaternion.identity).GetComponent<BaseLampLight>();
            lamp.InitialiseLamp(lampLight);
        }
    }

    public bool GetAllSceneLampsWork()
    {
        //if any lamp is broken return false
        foreach(Lamp lamp in levelLamps)
        {
            if (!lamp.GetIsLampWorking()) return false;
        }
        //if no one is return true
        return true;
    }
    //Gets the next nearest fuse
    public Transform GetNearestFuseLightFuse(Transform targetObject)
    {
        Transform nearestFuseTransform;

        //Set initial shortest distance (potentially make it random for polish)
        float currShortestDistance = Vector2.Distance(targetObject.position, levelLamps[0].GetLightFuse().transform.position);

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
        for (int i = 0; i < levelLamps.Count; i++)
        {
            //If the current lamp is working, compare distance
            if (levelLamps[i].GetIsLampWorking())
            {
                float distance;
                //If nearest transform equal null we can assume this is the first working light, Hence return this and make this the nearest
                if (nearestFuseTransform == false)
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

    //Gets a random working lamp and breaks it
    private void BreakRandomLamp()
    {
        List<Lamp> workingLamps = new List<Lamp>();//new list to store all working lamps

        for(int i = 0; i<levelLamps.Count; i++)
        {
            if (levelLamps[i].GetIsLampWorking()) workingLamps.Add(levelLamps[i]); 
        }

        int rand = Random.Range(0, workingLamps.Count);

        workingLamps[rand].BeginLampFlicker();
        OnLampBroke?.Invoke();
    }

    //Get percent of lamps working in level
    private float CalculatePercentageWorkingLamps()
    {
        int nWorking = 0;

        for(int i = 0; i < levelLamps.Count; i++)
        {
            if (levelLamps[i].GetIsLampWorking()) nWorking++;//for every working lamp the count is added
        }

        return nWorking / levelLamps.Count;//if 1, 100 percent works 0 none works
    }

    //compares number of working lamps to total lamps to determine if more lamps should be broken
    public void DetermineShouldBreakLight()
    {
        if(CalculatePercentageWorkingLamps() >= targetLightWorkingPercent)
        {
            shouldBreakLights = true;
        }
        else
        {
            shouldBreakLights = false;
        }
    }

    private void GetNewBreakLampTime()
    {
        currentTimeBeforeLightBreak = Random.Range(difficultySettings.minLampBreakTime, difficultySettings.maxLampBreakTime);
    }

 
    public void BindToInitManager()
    {
        InitStateManager.instance.OnStateChange += EvaluateNewState;
        GameStateManager.instance.OnGameStateChange += EvaluateGameNewState;
    }
    private void EvaluateNewState(InitStates newState)
    {
        switch (newState)
        {
           
            case InitStates.PlayerRespawned:
                FixAllSceneLamps();
                break;

            case InitStates.ExitLevel:

                break;
        }
    }
    private void EvaluateGameNewState(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.LevelClear:
                currentGameState = newState;
                FixAllSceneLamps();
                break;
            case GameStates.MainPowerOff:
                currentGameState = newState;
                break;
            case GameStates.TasksCompleted:
                currentGameState = newState;
                break;
        }
    }

    void IInitialisable.Init()
    {
        Init();
    }
}
