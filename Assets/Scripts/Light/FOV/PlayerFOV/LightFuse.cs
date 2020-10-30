﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class LightFuse : MonoBehaviour, IBreakable, Controls.IInteractionsActions
{
    private Lamp parentLamp;
    public FuseSettings fuseSettings;
    private bool canFix;
    private bool isFixing =false;
    private ChargingCable fixingCable;
    private Transform targetTrans;
    public float currentTimeToFix;
    private Controls input;

    private void Awake()
    {
        parentLamp = transform.parent.GetComponent<Lamp>();
        fixingCable = gameObject.GetComponent<ChargingCable>();

        //Inputs
        input = new Controls();
        input.Interactions.SetCallbacks(this);
        input.Enable();
    }

    private void Update()
    {

        if (isFixing)
        {
            if (!parentLamp.GetIsFixed())
            {
                if (currentTimeToFix <= 0)
                {
                    parentLamp.FixLamp(10f);
                    currentTimeToFix = fuseSettings.repairRate;
                }
                else
                {
                    currentTimeToFix -= Time.deltaTime;
                }
            }
            else
            {
                fixingCable.ChangeColour(Color.green);
                isFixing = false;
            }
            
           
        }
      
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isFixing)
            {
                targetTrans = other.gameObject.transform;
                Debug.Log(targetTrans);
                canFix = !parentLamp.GetIsLampWorking();
                if (canFix)
                {
                    fixingCable.ChangeColour(Color.yellow);
                }
                InGamePrompt.instance.ChangePrompt("[E] To Fix Light");
                InGamePrompt.instance.ShowPrompt();


            }
    
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!isFixing)
            {
                canFix = !parentLamp.GetIsLampWorking();

            }
         
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            canFix = !parentLamp.GetIsLampWorking();
            isFixing = false;
            InGamePrompt.instance.HidePrompt();
            if(targetTrans != null){

                fixingCable.StopDrawingRope();
                targetTrans = null;
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed&& canFix)
        {
            isFixing = true;
            InGamePrompt.instance.HidePrompt();
            if (targetTrans != null)
            {
                fixingCable.StartDrawingRope(targetTrans);

            }
        }
    }
    void IBreakable.Damage(float damage,BaseEnemy interfacingEnemy)
    {
        parentLamp.DamageLamp(damage);
        if (!parentLamp.GetIsLampWorking())
        {
            interfacingEnemy.GetComponent<IBreakable>().ObjectIsBroken();
        }
    }

    void IBreakable.ObjectIsBroken()
    {

    }

    void OnDestroy()
    {
        input.Disable();
    }
}