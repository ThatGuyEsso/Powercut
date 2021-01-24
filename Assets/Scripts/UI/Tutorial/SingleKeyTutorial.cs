using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class SingleKeyTutorial : BaseTutorial
{
    [SerializeField] private float triggeredOppacity;
    private enum InputActionType
    {
        SwitchWeapon,
        UseGadget,
        Reload,
        Shoot
    };

    [SerializeField]
    private Image keyPrompt;
    [SerializeField] private InputActionType inputActionType;






    //on shoot input action
    override public void OnShoot(InputAction.CallbackContext context)
    {
        if(inputActionType == InputActionType.Shoot&& context.performed && isActive)
        {
            keyPrompt.color = new Color(keyPrompt.color.r, keyPrompt.color.g, keyPrompt.color.b, triggeredOppacity);
   
            BeginEndTutorial();
        }
    }
    //on reload input action
    override public void OnReload(InputAction.CallbackContext context)
    {
        if (inputActionType == InputActionType.Reload && context.performed && isActive)
        {
            keyPrompt.color = new Color(keyPrompt.color.r, keyPrompt.color.g, keyPrompt.color.b, triggeredOppacity);
         
            BeginEndTutorial();
        }
    }

    //on Switch Weapon input action
    override public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
        if (inputActionType == InputActionType.SwitchWeapon && context.performed&&isActive)
        {
            keyPrompt.color = new Color(keyPrompt.color.r, keyPrompt.color.g, keyPrompt.color.b, triggeredOppacity);
   
            BeginEndTutorial();
        }
    }

    //One use gadget
    override public void OnUsePrimaryGadget(InputAction.CallbackContext context)
    {
        if (inputActionType == InputActionType.UseGadget && context.performed && isActive)
        {
            keyPrompt.color = new Color(keyPrompt.color.r, keyPrompt.color.g, keyPrompt.color.b, triggeredOppacity);
   
            BeginEndTutorial();
        }
    }

}
