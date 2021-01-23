using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseTutorial : BaseTutorial
{
    [SerializeField] private float timeToFadeEnd;
    [SerializeField] private float currentTimeToEnd;
    public override void Init()
    {
        base.Init();
        currentTimeToEnd = timeToFadeEnd;
    }
    private void Update()
    {
        if (isActive)
        {
            if(currentTimeToEnd<= 0)
            {
                BeginEndTutorial();
            }
            else
            {
                currentTimeToEnd -= Time.deltaTime;
            }
        }
    }
}
