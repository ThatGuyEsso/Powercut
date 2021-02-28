using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TaskTemplate : MonoBehaviour
{
    private string taskName;
    private TextMeshProUGUI taskDescription;
    private TextMeshProUGUI nTasksCompletedText;
    private TextMeshProUGUI nTotalTasksText;
    private int nTasksCompleted;
    private int nTotalTasks;
    private void Awake()
    {
        SetUpChildCompoents();
        Init();
    }

    public void ResetDisplay()
    {
        nTasksCompleted = 0;
        nTotalTasksText.text = nTotalTasks.ToString();
        nTasksCompletedText.text = nTasksCompleted.ToString();
    }

    //Increment and decremnt tasks
    //-------------------------------------------------------
    public void IncrementTotalTasks()
    {
        nTotalTasks++;
        nTotalTasksText.text = nTotalTasks.ToString();
    }
    public void DecrementTotalTasks()
    {
        nTotalTasks--;
        nTotalTasksText.text = nTotalTasks.ToString();
    }

    public void IncrementCompletedTasks()
    {
        nTasksCompleted++;
        nTasksCompletedText.text = nTasksCompleted.ToString();
    }
    public void DecrementCompletedTasks()
    {
        nTasksCompleted--;
        nTasksCompletedText.text = nTasksCompleted.ToString();
    }
    //-------------------------------------------------------

        //Setup functions
    //-------------------------------------------------------
    public void SetUpTemplate(string taskName)
    {
        this.taskName = taskName;
        taskDescription.text = this.taskName;
        //IncrementTotalTasks();
    }
    public void Init()
    {
        nTasksCompleted = 0;
        nTotalTasks = 0;
        nTotalTasksText.text = nTotalTasks.ToString();
        nTasksCompletedText.text = nTasksCompleted.ToString();
    }

    public void SetUpChildCompoents()
    {
        nTotalTasksText = transform.Find("Total Tasks").GetComponent<TextMeshProUGUI>();
        nTasksCompletedText = transform.Find("Tasks Completed").GetComponent<TextMeshProUGUI>();
        taskDescription = transform.Find("Task name").GetComponent<TextMeshProUGUI>();
    }
    //-------------------------------------------------------


    public string GetTaskName()
    {
        return taskName;
    }
}
