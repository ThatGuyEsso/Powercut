using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class BaseTutorial : MonoBehaviour, Controls.IPlayerControlsActions
{
    protected bool isActive =false;
    protected FadeMediaGroup fadeController;
    protected Controls input;
   [SerializeField] protected int nTutorialElements;


    public delegate void TutorialCompleteDelegate();
    public event TutorialCompleteDelegate OnTutorialComplete;
    virtual public void Init()
    {
        fadeController = gameObject.GetComponent<FadeMediaGroup>();
        fadeController.OnFadeComplete += Activate;
        input = new Controls();
        input.PlayerControls.SetCallbacks(this);
        input.Enable();
    }
    public void DisablePrompt()
    {
        BeginEndTutorial();
    }

    public void Activate(GameObject go)
    {
        isActive = true;
        fadeController.OnFadeComplete -= Activate;
    }


    protected void BeginEndTutorial() {
        fadeController.OnFadeComplete += EndTutorial;
        isActive = false;
        fadeController.BeginFadeOut();
    }

    protected void EndTutorial(GameObject go)
    {
        OnTutorialComplete?.Invoke();
        Destroy(gameObject);
    }
    //Get input action to move
    virtual public void OnMovement(InputAction.CallbackContext context) 
    {

    }

    //on shoot input action
    virtual public void OnShoot(InputAction.CallbackContext context)
    {
   
    }
    //on reload input action
    virtual public void OnReload(InputAction.CallbackContext context)
    {

    }

    //on Switch Weapon input action
    virtual public void OnSwitchWeapon(InputAction.CallbackContext context)
    {
     
    }

    //One use gadget
    virtual public void OnUsePrimaryGadget(InputAction.CallbackContext context)
    {

    }


    private void OnDestroy()
    {
        input.Disable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
