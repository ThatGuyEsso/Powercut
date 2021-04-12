using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDisplay : MonoBehaviour
{
    public GameObject eventTemplatePrefab;
    [SerializeField] private Transform origin;
    private int currNumEvents;
    public float timeBetweenClearing;
    public float maxTimeDelete;
    private float currentDeleteTime;

    [SerializeField] private Vector3 offset;
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
        


        currEvents.Add(newEvent);
        newEvent.SetUpEventTemplate(eventMessage, textColour);
        currNumEvents = currEvents.Count;
        StopCoroutine(ClearList());
        newEvent.transform.position =SetEventPosition(newEvent.rt.rect.height, newEvent.rt);
    }

    private Vector3 SetEventPosition(float eventTextHeight, RectTransform rect)
    {

        Vector3 offset = new Vector3(origin.position.x+rect.rect.width/2, origin.position.y - eventTextHeight * currNumEvents, origin.position.z);
        return offset;
      
    }
    private Vector3 GetEventPosition(float eventTextHeight, int index)
    {
        Vector3 offset = new Vector3(origin.position.x+ currEvents[index].rt.rect.width/2, origin.position.y - eventTextHeight * (index+1), origin.position.z);
        return offset;

    }

    private void RefreshPositions()
    {
        for(int i = 0;i< currEvents.Count; i++)
        {
            if(currEvents[i])
                currEvents[i].transform.position = GetEventPosition(currEvents[i].rt.rect.height, i);
        }
    }

    private IEnumerator ClearList()
    {
     
        while (currNumEvents > 0)
        {
            List<EventTemplate> eventsCopy = new List<EventTemplate>();
            eventsCopy.AddRange(currEvents);
            EventTemplate eventDisplay = eventsCopy[0];
            eventsCopy.Remove(eventDisplay);
            Destroy(eventDisplay.gameObject);
            currEvents.Clear();
            for ( int i = 0; i< eventsCopy.Count; i++){
                if (eventsCopy[i])
                    currEvents.Add(eventsCopy[i]);
            }
            currNumEvents--;
            RefreshPositions();
            yield return new WaitForSeconds(timeBetweenClearing);
        }

    }

}
