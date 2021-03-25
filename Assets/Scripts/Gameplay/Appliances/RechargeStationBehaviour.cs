using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RechargeStationBehaviour : MonoBehaviour
{
    private Transform playerTrans;
    private LightManager lightManger;
    private ChargingCable chargeCable;
    public Color ChargingColour;
    [SerializeField] protected GameObject audioPlayerPrefab;
    protected AudioPlayer audioPlayer;

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
            chargeCable.ChangeColour(ChargingColour);

            audioPlayer = ObjectPoolManager.Spawn(audioPlayerPrefab, transform.position, Quaternion.identity).GetComponent<AudioPlayer>();
            if (audioPlayer)
            {
                audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound("ChargingCableSFX"));
                audioPlayer.Play();
            }
    

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
                    if (lightManger.GetChargeState() != ChargeStates.StandBy)
                    {
                        if (audioPlayer)
                        {
                            audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound("ObjectFixed"));
                            audioPlayer.Play();
                            audioPlayer = null;
                        }
                     
                    }
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
            if (audioPlayer != false)
            {
                audioPlayer.KillAudio();
                audioPlayer = null;
            }
        }

    }




}
