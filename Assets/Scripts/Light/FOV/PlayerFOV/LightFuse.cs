using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFuse : MonoBehaviour, IBreakable
{
    private Lamp parentLamp;
    public FuseSettings fuseSettings;
    private bool canFix;
    private bool isFixing =false;
    private ChargingCable fixingCable;
    private Transform targetTrans;
    public float currentTimeToFix;

    private void Awake()
    {
        parentLamp = transform.parent.GetComponent<Lamp>();
        fixingCable = gameObject.GetComponent<ChargingCable>();
    }

    private void Update()
    {
        Debug.Log("press E");
        if (canFix)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isFixing = true;
                InGamePrompt.instance.HidePrompt();
                if (targetTrans != null)
                {
                  fixingCable.StartDrawingRope(targetTrans);

                }
            }

        }


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
            fixingCable.StopDrawingRope();
            targetTrans = null;
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
}
