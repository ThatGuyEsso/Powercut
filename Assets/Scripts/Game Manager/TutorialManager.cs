using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Tutorials")]
    [SerializeField] private FadeMediaGroup movementPrompt;
    [SerializeField] private FadeMediaGroup rotatePrompt;
    [SerializeField] private FadeMediaGroup switchWeaponPrompt;
    [SerializeField] private FadeMediaGroup shootPrompt;
    [SerializeField] private FadeMediaGroup reloadPrompt;
    [SerializeField] private FadeMediaGroup gadgetPrompt;
    [SerializeField] private FadeMediaGroup fusePrompt;
    [SerializeField] private FadeMediaGroup rechargePrompt;
    [SerializeField] private List<BaseTutorial> allTutorials;

    [Header("Management Settings")]
    [SerializeField] private float timebeforeTutorialStart =1.5f;
    [SerializeField] private int nTutorialsLeft;
    bool gameplayTutorialTriggered =false;


    [SerializeField] private TargetPointer pointerPrefab;
    private TargetPointer objectivePointer;
    [SerializeField] private TargetPointer rechargePointerPrefab;
    private TargetPointer rechargePointer;

    public void Init()
    {

        nTutorialsLeft = allTutorials.Count;

        foreach (BaseTutorial tutorial in allTutorials)
        {
            tutorial.OnTutorialComplete += DecrementTutorialsLeft;
        }

        GameStateManager.instance.OnGameStateChange += EvaluateGameState;
        Invoke("OrientationTutorial", timebeforeTutorialStart);
        SetUpObjectivePointer();




    }

    private void EvaluateGameState(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.MainPowerOff:

                if (!gameplayTutorialTriggered)
                {
                    gameplayTutorialTriggered = true;
                    BeginGamePlayTutoiral();
                    objectivePointer.SetCurrentTarget(TaskManager.instance.GetNearestBrokenTask(objectivePointer.transform));
                }
                break;
            case GameStates.LevelClear:
                objectivePointer.SetCurrentTarget(FindObjectOfType<Car>().transform);
                Debug.Log("new Pointer");
                break;
        }
    }

    private void OrientationTutorial()
    {
        movementPrompt.gameObject.SetActive(true);
        movementPrompt.gameObject.GetComponent<MovementTutorial>().Init();
        movementPrompt.BeginFadeIn();
        rotatePrompt.gameObject.SetActive(true);
        rotatePrompt.gameObject.GetComponent<MouseTutorial>().Init();
        rotatePrompt.BeginFadeIn();
    }


    private void BeginGamePlayTutoiral()
    {
        //if the previous tutorial is not visible begin displaying the new
        if(!movementPrompt!=false && !rotatePrompt !=false)
        {
            GamePlayTutoiral(null);
        }
        else
        {
            //if this prompt is active and already fading
            if(movementPrompt ==true && !movementPrompt.GetIsFading())
            {
                movementPrompt.BeginFadeOut();
                movementPrompt.gameObject.GetComponent<MovementTutorial>().DisablePrompt();
                movementPrompt.OnFadeComplete += GamePlayTutoiral;


            }
            //if this prompt is active and already fading
            if (rotatePrompt ==true && !rotatePrompt.GetIsFading())
            {
                rotatePrompt.BeginFadeOut();
                rotatePrompt.gameObject.GetComponent<MouseTutorial>().DisablePrompt();
                rotatePrompt.OnFadeComplete += GamePlayTutoiral;

            }
        }
        LevelLampsManager.instance.OnLampBroke += LightFixingPrompt;
        LightManager.instance.OnChargeDepleted += RechargePrompt;
        TaskManager.instance.OnAllTasksCompletd += TaskCompletedPrompt;
    }

    private void TaskCompletedPrompt()
    {
        TaskManager.instance.OnAllTasksCompletd -= TaskCompletedPrompt;
        InGamePrompt.instance.SetColor(Color.green);
        InGamePrompt.instance.ShowPromptTimer("Job completed Turn Main Power Back On",4.0f);
        objectivePointer.SetCurrentTarget(FindObjectOfType<MainPowerSwitch>().transform);
    }
    private void LightFixingPrompt()
    {
        LevelLampsManager.instance.OnLampBroke -= LightFixingPrompt;
        fusePrompt.gameObject.SetActive(true);
        fusePrompt.gameObject.GetComponent<MouseTutorial>().Init();
        fusePrompt.BeginFadeIn();
    }

    private void DisableRechargePointer()
    {
        LightManager.instance.OnFullyCharged -= DisableRechargePointer;
        Destroy(rechargePointer.gameObject);
    }

    private void RechargePrompt()
    {
        LightManager.instance.OnChargeDepleted -= RechargePrompt;
        LightManager.instance.OnFullyCharged += DisableRechargePointer;
        rechargePrompt.gameObject.SetActive(true);
        rechargePrompt.gameObject.GetComponent<MouseTutorial>().Init();
        SetUpRechargePointer();
        rechargePrompt.BeginFadeIn();

    }
    private void GamePlayTutoiral(GameObject go)
    {
        //unbind if bound
        if(rotatePrompt !=false) rotatePrompt.OnFadeComplete -= GamePlayTutoiral;
        if (movementPrompt != false) movementPrompt.OnFadeComplete -= GamePlayTutoiral;
        //weapon switch prompt
        switchWeaponPrompt.gameObject.SetActive(true);
        switchWeaponPrompt.gameObject.GetComponent<SingleKeyTutorial>().Init();
        switchWeaponPrompt.BeginFadeIn();

        //shoot weapon prompt
        shootPrompt.gameObject.SetActive(true);
        shootPrompt.gameObject.GetComponent<SingleKeyTutorial>().Init();
        shootPrompt.BeginFadeIn();

        WeaponManager.instance.OnClipEmpty += OutOfAmmoTutorial;
    }


    private void OutOfAmmoTutorial()
    {

        WeaponManager.instance.OnClipEmpty -= OutOfAmmoTutorial;
        //reload prompt
        reloadPrompt.gameObject.SetActive(true);
        reloadPrompt.gameObject.GetComponent<SingleKeyTutorial>().Init();
        reloadPrompt.BeginFadeIn();

        //use gadget prompt
        gadgetPrompt.gameObject.SetActive(true);
        gadgetPrompt.gameObject.GetComponent<SingleKeyTutorial>().Init();
        gadgetPrompt.BeginFadeIn();
    }

    private void DecrementTutorialsLeft()
    {
        nTutorialsLeft--;
        if(nTutorialsLeft <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void SetUpObjectivePointer()
    {
        Transform playerTransform = FindObjectOfType<PlayerBehaviour>().transform;
        objectivePointer = Instantiate(pointerPrefab, playerTransform.position, Quaternion.identity);
        objectivePointer.Init(playerTransform, FindObjectOfType<MainPowerSwitch>().transform);
        objectivePointer.ActivatePointer();
    }
    private void SetUpRechargePointer()
    {
        Transform playerTransform = FindObjectOfType<PlayerBehaviour>().transform;
        rechargePointer = Instantiate(rechargePointerPrefab, playerTransform.position, Quaternion.identity);
        rechargePointer.Init(playerTransform, FindObjectOfType<RechargeStationBehaviour>().transform);
        rechargePointer.ActivatePointer();
    }
    private void OnDestroy()
    {

        GameStateManager.instance.OnGameStateChange -= EvaluateGameState;
    }
}
