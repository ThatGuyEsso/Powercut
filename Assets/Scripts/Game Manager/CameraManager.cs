using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Timeline;
using UnityEngine.Playables ;
public class CameraManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera cinematicCamera;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private PlayableAsset bossIntro;
    [SerializeField] private PlayableAsset ReturnToPlayer;
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
        InitStateManager.currGameMode = GameModes.Cutscene;
        director.playableAsset = bossIntro;
        director.Play();
        CamShake.instance.gameObject.SetActive(false);
        

    }

    public void ReturnCutScene()
    {
        director.playableAsset = ReturnToPlayer;
        director.Play();
    }
    public void SwitchToPlayerCam()
    {
        InitStateManager.currGameMode = GameModes.Powercut;
        GameStateManager.instance.BeginNewGameState(GameStates.MainPowerOff);
        cinematicCamera.gameObject.SetActive(false);
        CamShake.instance.gameObject.SetActive(true);
        director.Stop();
        director.enabled = false;
    }
}
