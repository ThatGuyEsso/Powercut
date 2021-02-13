using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirConditionTask : BaseTask
{




    override protected void EvaluateSpriteDisplay()
    {
        if (isFixed) gfx.sprite = stateSprites[0];
        else if( isFixing) gfx.sprite = stateSprites[1];
        else gfx.sprite = stateSprites[2];
    }
}
