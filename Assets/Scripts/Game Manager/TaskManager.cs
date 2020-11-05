using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance;

    //Task Data
    private List<BaseTask> allTasks= new List<BaseTask>(); //Stores all task instances
    private List<string> taskNames =new List<string>(); //Stores individual task names
    private int totalNumberOfCompletedTask; //How many task have been completed 


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
        GetTasksInLevel();
    }

  

    private void GetTasksInLevel()
    {
        BaseTask[] tasks = FindObjectsOfType<BaseTask>();//Get everything that inherits tasks
        taskNames.Add(tasks[0].GetTaskName());
         //Add all tasks to list of tasks
        for (int i = 0; i < tasks.Length; i++)
        {
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


        //Update UI
        UIManager.instance.taskDisplay.SetUpTasks(taskNames);
        UIManager.instance.taskDisplay.PopulateTasks(taskNames.Count, allTasks);
        Debug.Log("Tasks: " +taskNames.Count);
    }
}
