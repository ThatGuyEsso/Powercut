using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RechargeStationBehaviour : MonoBehaviour
{
    private Transform playerTrans;
    private LightManager lightManger;
    private ChargingCable chargeCable;

    private void Awake()
    {
        chargeCable = gameObject.GetComponentInChildren<ChargingCable>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            playerTrans = other.transform;
            lightManger = other.gameObject.GetComponent<PlayerBehaviour>().fieldOfView.GetComponent<FieldOfView>().GetLightManager();

            lightManger.SetChargeState(ChargeStates.Charging);
            chargeCable.StartDrawingRope(playerTrans);


        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (lightManger != null)
            {
                if (lightManger.GetIsFullyCharged())
                {
                    chargeCable.ChangeColour(Color.green);
                    lightManger.SetChargeState(ChargeStates.StandBy);
                }
            }
        }

    }

    public void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            if (lightManger != null)
            {
                lightManger.SetChargeState(ChargeStates.Discharging);

            }

            lightManger = null;
            playerTrans = null;

           chargeCable.StopDrawingRope();
        }

    }




}
