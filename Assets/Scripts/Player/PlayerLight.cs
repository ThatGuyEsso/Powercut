using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLight : MonoBehaviour,IPlayerComponents
{
    public float shrinkRate;
    private bool shouldShrink =false;
    private Vector3 newScale;
    private Vector3 initialScale;
   
    private void Awake()
    {
        newScale = transform.localScale;
        initialScale = transform.localScale;
    }

    private void FixedUpdate()
    {
        if (shouldShrink)
        {
            
            newScale = Vector3.Lerp(newScale, Vector3.zero, Time.deltaTime*shrinkRate);
            if(newScale.x < 0.05f)
            {
                newScale = Vector3.zero;
                shouldShrink = false;
            }
            shrinkRate += 0.1f;
            transform.localScale = newScale;

        }
    }
    void IPlayerComponents.PlayerDied()
    {
        shouldShrink = true;

    }
    void IPlayerComponents.PlayerReset()
    {
        shouldShrink = false;
        transform.localScale = initialScale;
    }
}
