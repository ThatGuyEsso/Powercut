using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GadgetCounter : MonoBehaviour
{
    public List<GameObject> counters = new List<GameObject>();
    private int currCounter;
    private int maxCounter;
    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        int counter = transform.childCount;
        for(int i = 0; i < counter; i++)
        {
            counters.Add(transform.GetChild(i).gameObject);
        }
    }


    public void IncrementCounter()
    {
        currCounter++;
        UpdateCounterDisplay();

    }
    public void DecrementCounter()
    {
        currCounter--;
        if (currCounter < 0)
        {
            currCounter = 0;
        }

        UpdateCounterDisplay();
    }

    public void UpdateCounterDisplay()
    {
        for(int i =0; i< counters.Count; i++)
        {
            
            if (i < currCounter)
            {
                counters[i].SetActive(true);
            }
            else
            {
                counters[i].SetActive(false);
            }
        }
    }

    public void ResetCounter()
    {
        currCounter = maxCounter;
        UpdateCounterDisplay();
    }
    public void SetUpCounter(int count)
    {
        maxCounter = count;
        currCounter = maxCounter;
    }


}
