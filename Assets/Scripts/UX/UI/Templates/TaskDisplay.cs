using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDisplay : MonoBehaviour
{
    [Header("Postioning Settings")]
    [SerializeField] private Vector2 offset;
    

    [Header("Display Settings")]
    public GameObject taskTemplatePrefab;
    private List<TaskTemplate> tasks = new List<TaskTemplate>();
    public int nTypesOfTasks; //stores the number of different tasks.

    //Rect transforms
    [SerializeField] private Transform taskArea;
    [SerializeField] private RectTransform taskStartPositon;
    RectTransform prevousTaskTransform;


    public Transform phone;
    public Transform offPositon;

    bool isinitiated = false;

    public void Init()
    {
        PauseScreen.instance.OnPause += UnHide;
        PauseScreen.instance.OnResume += Hide;
        isinitiated = true;
    }

    public void ResetTasks()
    {
        foreach(TaskTemplate task in tasks)
        {
            task.ResetDisplay();
        }
    }
    public void GenerateNewTaskDisplay()
    {
        
        TaskTemplate newTemplate = Instantiate(taskTemplatePrefab, transform.position,Quaternion.identity).GetComponent<TaskTemplate>();
        newTemplate.transform.SetParent(taskArea);
        RectTransform newTaskTransfrom = newTemplate.GetComponent<RectTransform>();


        if (prevousTaskTransform!=false)
        {
            float newHeight = newTaskTransfrom.rect.height;
            float prevHeight = prevousTaskTransform.rect.height;
            newTaskTransfrom.position = new Vector2(prevousTaskTransform.position.x, prevousTaskTransform.position.y - (newHeight/2) - (prevHeight)) + offset;
            prevousTaskTransform = newTaskTransfrom;

        }
        else
        {
           
            float height = newTaskTransfrom.rect.height;

            newTaskTransfrom.position = new Vector2(taskStartPositon.position.x, taskStartPositon.position.y - height / 2)+ offset;
            prevousTaskTransform = newTaskTransfrom;

        }
        tasks.Add(newTemplate);
    }

    public void SetUpTasks(List<string> listOfTaskNames)
    {
     
        for (int i = 0; i < listOfTaskNames.Count; i++)
        {
            GenerateNewTaskDisplay();
           
        }
        for (int i = 0; i < listOfTaskNames.Count; i++)
        {
            
            tasks[i].SetUpTemplate(listOfTaskNames[i]);
        }

    }
    public void ClearTaskBar()
    {
        foreach(TaskTemplate task in tasks)
        {
            Destroy(task.gameObject);
        }
        nTypesOfTasks = 0;
        tasks.Clear();
    }

    //Set Up A task
    public void PopulateTasks(int nTaskTypes, List<BaseTask> listOfTasks)
    {
        //Resize to number of task types
        nTypesOfTasks = nTaskTypes;
      

        //Go through each type of task
        for (int i =0; i < nTaskTypes; i++)
        {
            //Create a new task display item
            
            //Go through each task and assign it to correct task;
            for (int j =0; j < listOfTasks.Count; j++)
            {
                if (tasks[i].GetTaskName() == listOfTasks[j].GetTaskName())
                {
                    tasks[i].IncrementTotalTasks();
                }
            }
        }
    }

    public void UpdateCompletedTasks(bool isCompleted,string taskName)
    {
        if (isCompleted)
        {
            foreach(TaskTemplate task in tasks)
            {
                if(task.GetTaskName() == taskName)
                {
                    task.IncrementCompletedTasks();
                }
            }
        }
        else
        {
            foreach (TaskTemplate task in tasks)
            {
                if (task.GetTaskName() == taskName)
                {
                    task.DecrementCompletedTasks();
                }
            }
        }
    }

    private void Hide()
    {
        phone.position = offPositon.position;
    }

    private void UnHide()
    {
        phone.position = transform.position;
    }

    private void OnDestroy()
    {
        if (isinitiated)
        {
            PauseScreen.instance.OnPause -= UnHide;
            PauseScreen.instance.OnResume -= Hide;
        }
    }
}
