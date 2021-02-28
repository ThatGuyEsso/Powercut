using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class MovementTutorial :BaseTutorial
{
    [SerializeField] private Image leftMovement, rightMovement, forwardMovement, backwardMovement;
    private bool leftIsActive = true, rightIsActive = true, forwardIsActive = true, backwardIsActive = true;
    [SerializeField] private float triggeredOppacity;





    public override void OnMovement(InputAction.CallbackContext context)
    {
        if(context.performed && isActive)
        {
            if (context.ReadValue<Vector2>().x > 0&& rightIsActive)
            {
                rightIsActive = false;
                rightMovement.color = new Color(rightMovement.color.r,
               rightMovement.color.g, rightMovement.color.b, triggeredOppacity);
                nTutorialElements--;
            }
            else if (context.ReadValue<Vector2>().x < 0&& leftIsActive) {
                leftIsActive = false;
                leftMovement.color = new Color(leftMovement.color.r, leftMovement.color.g, leftMovement.color.b, triggeredOppacity);
                nTutorialElements--;
            }
            else if (context.ReadValue<Vector2>().y < 0&&backwardIsActive)
            {
                backwardIsActive = false;
                backwardMovement.color = new Color(backwardMovement.color.r, backwardMovement.color.g, backwardMovement.color.b, triggeredOppacity);
                nTutorialElements--;
            }else if (context.ReadValue<Vector2>().y > 0&&forwardIsActive)
            {
                forwardIsActive = false;
                forwardMovement.color = new Color(forwardMovement.color.r, forwardMovement.color.g, forwardMovement.color.b, triggeredOppacity);
                nTutorialElements--;
            }


            if(nTutorialElements <= 0)
            {
                BeginEndTutorial();
            }
        }
    }

}
