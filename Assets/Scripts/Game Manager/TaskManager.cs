using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour, IInitialisable
{
    public static TaskManager instance;

    //Task Data
    private List<BaseTask> allTasks= new List<BaseTask>(); //Stores all task instances
    private List<string> taskNames =new List<string>(); //Stores individual task names
    private int totalNumberOfCompletedTask; //How many task have been completed 


    public delegate void AllTaskCompletedDelegate();
    public event AllTaskCompletedDelegate OnAllTasksCompletd;
    public delegate void TaskDestroyedDelegate();
    public event TaskDestroyedDelegate OnTaskDestroyed;
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
    private void Init()
    {
        GetTasksInLevel();
        BindToInitManager();
    }


    public void RecordCompletedTask(string taskName)
    {
        if(totalNumberOfCompletedTask < allTasks.Count)
        {
            totalNumberOfCompletedTask++;
        }
        else
        {
            //all tasks already completed so return
            return;
        }
      
        UIManager.instance.taskDisplay.UpdateCompletedTasks(true, taskName);
        if (totalNumberOfCompletedTask >= allTasks.Count)
        {
            //All task Completed so tell game state manager 
            GameStateManager.instance.BeginNewGameState(GameStates.TasksCompleted);
            OnAllTasksCompletd?.Invoke();
        }
    }
    public void RecordFailedTask(string taskName)
    {
        if (totalNumberOfCompletedTask >0)
        {
            totalNumberOfCompletedTask--;
        }
        else
        {
            //no tasks have been completed yet so return
            return;
        }

        UIManager.instance.taskDisplay.UpdateCompletedTasks(false, taskName);
        if (totalNumberOfCompletedTask <= allTasks.Count)
        {
            //Not all tasks are completed so power is off and all lights should continue breaking
            GameStateManager.instance.BeginNewGameState(GameStates.MainPowerOff);
            OnTaskDestroyed?.Invoke();
        }
    }


    private void GetTasksInLevel()
    {
        BaseTask[] tasks = FindObjectsOfType<BaseTask>();//Get everything that inherits tasks

        taskNames.Add(tasks[0].GetTaskName());
         //Add all tasks to list of tasks
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i].Init();
            bool exists = false;
            
            for(int j = 0; j < taskNames.Count; j++)
            {
                if(tasks[i].GetTaskName() == taskNames[j])
                {
                    exists = true;
                }
            }

            if (!exists)
            {
                taskNames.Add(tasks[i].GetTaskName());
            }

            allTasks.Add(tasks[i]);
        }
        Debug.Log("Number of tasks " + allTasks.Count);

        //Update UI
        UIManager.instance.taskDisplay.SetUpTasks(taskNames);
        UIManager.instance.taskDisplay.PopulateTasks(taskNames.Count, allTasks);
        Debug.Log("Tasks: " +taskNames.Count);
    }
    private void ResetTasks()
    {
        foreach(BaseTask task in allTasks)
        {
            task.ResetTask();
        }
      
       
    }

    void IInitialisable.Init()
    {
        Init();
    }
    public void BindToInitManager()
    {
        InitStateManager.instance.OnStateChange += EvaluateNewState;
    }
    private void EvaluateNewState(InitStates newState)
    {
        switch (newState)
        {
            case InitStates.RespawnPlayer:
                ResetTasks();
             
                Debug.Log("init");
                break;
        }
    }
    public Transform GetNearestTask(Transform targetObject)
    {
        Transform nearestTaskT;

        //Set initial shortest distance (potentially make it random for polish)
        float currShortestDistance = Vector2.Distance(targetObject.position, allTasks[0].transform.position);

        //If the initial task is working
        if (allTasks[0].GetIsFixed())
        {
            //the nearest transfrom is its fuse
            nearestTaskT = allTasks[0].transform;
        }
        else
        {
            //Else the transform is null
            nearestTaskT = null;

        }
        for (int i = 0; i < allTasks.Count; i++)
        {
            //If the current tasks is working, compare distance
            if (allTasks[i].GetIsFixed())
            {
                float distance;
                //If nearest transform equal null we can assume this is the first working task, Hence return this and make this the nearest
                if (nearestTaskT == false)
                {
                    currShortestDistance = Vector3.Distance(targetObject.position, allTasks[i].transform.position);
                    nearestTaskT = allTasks[i].transform;
                }
                else
                {
                    distance = Vector2.Distance(targetObject.position, allTasks[i].transform.position);
                    if (distance < currShortestDistance)
                    {
                        currShortestDistance = distance;
                        nearestTaskT = allTasks[i].transform;
                    }
                }

            }


        }

        return nearestTaskT;
    }

    public Transform GetNearestBrokenTask(Transform targetObject)
    {
        Transform nearestTaskT;

        //Set initial shortest distance (potentially make it random for polish)
        float currShortestDistance = Vector2.Distance(targetObject.position, allTasks[0].transform.position);

        //If the initial task is working
        if (!allTasks[0].GetIsFixed())
        {
            //the nearest transfrom is its fuse
            nearestTaskT = allTasks[0].transform;
        }
        else
        {
            //Else the transform is null
            nearestTaskT = null;

        }
        for (int i = 0; i < allTasks.Count; i++)
        {
            //If the current tasks is working, compare distance
            if (!allTasks[i].GetIsFixed())
            {
                float distance;
                //If nearest transform equal null we can assume this is the first working task, Hence return this and make this the nearest
                if (nearestTaskT == false)
                {
                    currShortestDistance = Vector3.Distance(targetObject.position, allTasks[i].transform.position);
                    nearestTaskT = allTasks[i].transform;
                }
                else
                {
                    distance = Vector2.Distance(targetObject.position, allTasks[i].transform.position);
                    if (distance < currShortestDistance)
                    {
                        currShortestDistance = distance;
                        nearestTaskT = allTasks[i].transform;
                    }
                }

            }


        }

        return nearestTaskT;
    }

}
