using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Car : MonoBehaviour, Controls.IInteractionsActions,IAudio
{

    [SerializeField] private GameObject openCarGFX;
    [SerializeField] private GameObject closedCarGFX;
    [SerializeField] private GameObject audioPlayerPrefab;
    [SerializeField] private Transform spawnPosition;
    private Controls input;
    private bool inRange;
    private bool hasActivated =false;

    private void Awake()
    {
        openCarGFX.SetActive(false);
        //Inputs
        input = new Controls();
        input.Interactions.SetCallbacks(this);
        input.Enable();
    }



    private void OpecnCar()
    {
        openCarGFX.SetActive(true);
        closedCarGFX.SetActive(false);
        AudioManager.instance.PlayRandFromGroup("CarOpenSFX");
    }
    private void CloseCar()
    {
        openCarGFX.SetActive(false);
        closedCarGFX.SetActive(true);
        AudioManager.instance.PlayRandFromGroup("CarCloseSFX");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && GameStateManager.instance.GetCurrentGameState()== GameStates.LevelClear )
        {
            inRange = true;
            InGamePrompt.instance.ChangePrompt("[E] Enter Car");
            OpecnCar();
          

        }


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && GameStateManager.instance.GetCurrentGameState() == GameStates.LevelClear)
        {
            inRange = false;
            InGamePrompt.instance.HidePrompt();
            CloseCar();
        }


    }


    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed && GameStateManager.instance.GetCurrentGameState() == GameStates.LevelClear&&inRange&& hasActivated==false)
        {
            IAudio audioPlayer = ObjectPoolManager.Spawn(audioPlayerPrefab, transform.position).GetComponent<IAudio>();
            audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound("CarEngineSFX"));
            audioPlayer.Play();

            InitStateManager.instance.BeginNewState(InitStates.LoadTitleScreen);
            InGamePrompt.instance.HidePrompt();
            InGamePrompt.instance.ShowPrompt();
            hasActivated = true;
        }
    }


    public Transform GetSpawn()
    {
        if (spawnPosition != false) return spawnPosition;
        else return null;
    }

    public void SetUpAudioSource(Sound sound)
    {
        throw new System.NotImplementedException();
    }

    public void Play()
    {
        throw new System.NotImplementedException();
    }

    public void PlayAtRandomPitch()
    {
        throw new System.NotImplementedException();
    }
}
