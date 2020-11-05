using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskDisplay : MonoBehaviour
{
    [Header("Postioning Settings")]
    public float taskYOffset;
    public float taskXOffset;
    public float panelPadding;

    [Header("Display Settings")]
    public GameObject taskTemplatePrefab;
    private List<TaskTemplate> tasks = new List<TaskTemplate>();
    public int nTypesOfTasks; //stores the number of different tasks.

    //Rect transforms
    private RectTransform backgroundPanel;
    private RectTransform rectTrans;
    private void Awake()
    {
        //Cache referencs
        backgroundPanel = transform.Find("BackGround").GetComponent<RectTransform>();
        rectTrans = gameObject.GetComponent<RectTransform>();
       

    }



    //Size the panel to fit all tasks
    private void ResizePanelHeight()
    {
        float size = (panelPadding * nTypesOfTasks);
        backgroundPanel.offsetMin = new Vector2(0f, size);
    }
    private void GenerateNewTaskDisplay(int indexPosition)
    {
        TaskTemplate newTemplate = Instantiate(taskTemplatePrefab, transform.position,Quaternion.identity).GetComponent<TaskTemplate>();
        newTemplate.transform.parent = transform;
        RectTransform templateRect = newTemplate.gameObject.GetComponent<RectTransform>();
        
        if (templateRect == true)
        {

            Vector2 targetPos = rectTrans.transform.position;
            
           
            templateRect.anchoredPosition = new Vector2(0, taskYOffset + (taskYOffset*indexPosition)/2);
           
        }
        tasks.Add(newTemplate);
    }

    public void SetUpTasks(List<string> listOfTaskNames)
    {
     
        for (int i = 0; i < listOfTaskNames.Count; i++)
        {
            GenerateNewTaskDisplay(i);
           
        }
        for (int i = 0; i < listOfTaskNames.Count; i++)
        {
            
            tasks[i].SetUpTemplate(listOfTaskNames[i]);
        }

    }

    //Set Up A task
    public void PopulateTasks(int nTaskTypes, List<BaseTask> listOfTasks)
    {
        //Resize to number of task types
        nTypesOfTasks = nTaskTypes;
        ResizePanelHeight();

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


  
}
