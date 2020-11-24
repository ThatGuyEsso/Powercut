using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDisplay : MonoBehaviour
{
    public GameObject eventTemplatePrefab;
    public Vector3 origin;
    private int currNumEvents;
    public float timeBetweenClearing;
    public float maxTimeDelete;
    private float currentDeleteTime;

    private List<EventTemplate> currEvents = new List<EventTemplate>();

    public void Update()
    {
        if(currNumEvents > 0)
        {
            if (currentDeleteTime <= 0)
            {
                StartCoroutine(ClearList());
                currentDeleteTime = maxTimeDelete;
            }
            else
            {
                currentDeleteTime-=Time.deltaTime;
            }
        }
    }


    public void CreateEvent(string eventMessage,Color textColour)
    {
        currentDeleteTime = maxTimeDelete;
        EventTemplate newEvent = Instantiate(eventTemplatePrefab, transform).GetComponent< EventTemplate>();
        newEvent.Init();

        currEvents.Add(newEvent);
        newEvent.SetUpEventTemplate(eventMessage, textColour);
        currNumEvents = currEvents.Count;
        StopCoroutine(ClearList());
        newEvent.transform.localPosition =GetEventPosition(newEvent.height);
    }

    private Vector3 GetEventPosition(float eventTextHeight)
    {
        Vector3 offset = new Vector3(origin.x, origin.y - eventTextHeight * currNumEvents, origin.z);
        return offset;
      
    }
    private Vector3 GetEventRelativePosition(float eventTextHeight, float index)
    {
        Vector3 offset = new Vector3(origin.x, origin.y - eventTextHeight * index, origin.z);
        return offset;

    }

    private void RefreshPositions()
    {
        for(int i = 0;i< currEvents.Count; i++)
        {
            currEvents[i].transform.localPosition = GetEventRelativePosition(currEvents[i].height, i+1);
        }
    }

    private IEnumerator ClearList()
    {
     
        while (currNumEvents > 0)
        {
           
            GameObject eventDisplay = currEvents[0].gameObject;
            currEvents.Remove(currEvents[0]);
            Destroy(eventDisplay);
            RefreshPositions();
            currNumEvents--;
            yield return new WaitForSeconds(timeBetweenClearing);
        }

    }

}
