using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIntensityManager : MonoBehaviour, IInitialisable
{
    public static GameIntensityManager instance;
    [SerializeField] private LevelDifficultyData settings;
    int nCrawlers;

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
    }

    public void IncrementNumberOfCrawlers()
    {
        nCrawlers++;
        if (nCrawlers >= settings.maxNumberCrawlers) OnLimitReached?.Invoke();

    }

    public void DecrementNumberOfCrawlers()
    {

        //if at the limit
        if (nCrawlers == settings.maxNumberCrawlers)
        {
            //decrement number of crawlers
            nCrawlers--;
            //invoke it not longer is at the max
            OnNotAtMax?.Invoke();
        }
        else nCrawlers--;//otherwise just decrement as usual


    }

    public delegate void LimitDelegate();
    public event LimitDelegate OnLimitReached;
    public event LimitDelegate OnNotAtMax;

    public bool GetIsAtCrawlerLimit() { return nCrawlers >= settings.maxNumberCrawlers; }

    void IInitialisable.Init()
    {
        Init();
    }
}
