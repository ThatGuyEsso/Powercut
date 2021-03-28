using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Timeline;
public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera cinematicCamera;
    public PlayableTrack track;
    public static CameraManager instance;
    private void Awake()
    {
        if (instance == false)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void SwitchToCutScene()
    {
    
        CamShake.instance.gameObject.SetActive(false);
        

    }
    public void SwitchToPlayerCam()
    {
       
        CamShake.instance.gameObject.SetActive(true);
    }
}
