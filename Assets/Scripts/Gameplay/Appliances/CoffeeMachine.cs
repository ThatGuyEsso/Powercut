using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeMachine : AirConditionTask
{

    override protected void EvaluateSpriteDisplay()
    {
        float percentageHealth = currHealth / maxHealth;
        if (!isFixed)
        {
            if (percentageHealth <= 0.25f)
            {
                gfx.sprite = stateSprites[stateSprites.Length - 1];
            }
            else if (percentageHealth > 0.25f && percentageHealth <= 0.5f)
            {
                gfx.sprite = stateSprites[stateSprites.Length - 2];
            }
            else if (percentageHealth > 0.5f && percentageHealth <= 0.75f)
            {
                gfx.sprite = stateSprites[stateSprites.Length - 3];
            }
        }
        else
        {
          
            gfx.sprite = stateSprites[0];
           
        }

    }

}
