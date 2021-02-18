using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleAutomaticDoor : AutomaticDoor
{
    protected override void Update()
    {
        switch (state)
        {

            case DoorState.Opening:
                if (leftDoorTransform != false)
                {
                    leftDoorTransform.position += (Vector3)(-moveAxis * transitionTime * Time.deltaTime);
                    if (Vector2.Distance(leftDoorTransform.position, leftOpenPosition) <= 0.01f)
                    {
                            state = DoorState.Open;
                    }
                }

                if (rightDoorTransform != false)
                {
                    rightDoorTransform.position += (Vector3)(moveAxis * transitionTime * Time.deltaTime);
                    if (Vector2.Distance(rightDoorTransform.position, rightOpenPosition) <= 0.01f) state = DoorState.Open;
                }

                break;
            case DoorState.Closing:
                if (leftDoorTransform != false)
                {
                    leftDoorTransform.position -= (Vector3)(-moveAxis * transitionTime * Time.deltaTime);
                    if ((Vector2.Distance(leftDoorTransform.position, leftClosedPosition) <= 0.01f))
                    {
                        state = DoorState.Closed;
                    }
                }

                if (rightDoorTransform != false)
                {
                    rightDoorTransform.position -= (Vector3)(moveAxis * transitionTime * Time.deltaTime);
                    if (Vector2.Distance(rightDoorTransform.position, rightClosedPosition) <= 0.01f)
                    {
                        state = DoorState.Closed;
                    }

                }
                break;
        }
    }
}