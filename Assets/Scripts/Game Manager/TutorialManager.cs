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
    [SerializeField] private FadeMediaGroup taskCompletionPrompts;
    [SerializeField] private List<BaseTutorial> allTutorials;

    [Header("Management Settings")]
    [SerializeField] private float timebeforeTutorialStart =1.5f;
    [SerializeField] private int nTutorialsLeft;
    bool gameplayTutorialTriggered =false;




    public static TutorialManager instance;

    
    public void Init()
    {
        if (instance == false)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
        nTutorialsLeft = allTutorials.Count;

        foreach (BaseTutorial tutorial in allTutorials)
        {
            tutorial.OnTutorialComplete += DecrementTutorialsLeft;
        }

        GameStateManager.instance.OnGameStateChange += EvaluateGameState;
        InitStateManager.instance.OnStateChange += EvaluateInitState;
        Invoke("OrientationTutorial", timebeforeTutorialStart);
        //SetUpObjectivePointer();




    }

    private void EvaluateInitState(InitStates newState)
    {
        switch (newState)
        {
            case InitStates.LoadTitleScreen:
                Destroy(gameObject);
                break;
            case InitStates.LoadMainMenu:
                Destroy(gameObject);
                break;

        }
    }
    private void EvaluateGameState(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.MainPowerOff:

                if (!gameplayTutorialTriggered)
                {
                    gameplayTutorialTriggered = true;
                    BeginGamePlayTutorial();

                }
                break;
            case GameStates.LevelClear:
 
      
                break;
        }
    }

    public void OrientationTutorial()
    {
        movementPrompt.gameObject.SetActive(true);
        movementPrompt.gameObject.GetComponent<MovementTutorial>().Init();
        movementPrompt.BeginFadeIn();
        rotatePrompt.gameObject.SetActive(true);
        rotatePrompt.gameObject.GetComponent<MouseTutorial>().Init();
        rotatePrompt.BeginFadeIn();
    }


    private void BeginGamePlayTutorial()
    {
        ClearActivePrompts();
        DisplayObjectives();


        LevelLampsManager.instance.OnLampBroke += BeginFightTutorial;
        LightManager.instance.OnChargeDepleted += RechargePrompt;
        TaskManager.instance.OnAllTasksCompletd += TaskCompletedPrompt;
    }

    private void TaskCompletedPrompt()
    {
        TaskManager.instance.OnAllTasksCompletd -= TaskCompletedPrompt;
        InGamePrompt.instance.SetColor(Color.green);
        InGamePrompt.instance.ShowPromptTimer("Job completed Turn Main Power Back On",4.0f);
        //objectivePointer.SetCurrentTarget(FindObjectOfType<MainPowerSwitch>().transform);
    }
    private void LightFixingPrompt()
    {
        LevelLampsManager.instance.OnLampBroke -= LightFixingPrompt;
        fusePrompt.gameObject.SetActive(true);
        fusePrompt.gameObject.GetComponent<MouseTutorial>().Init();
        fusePrompt.BeginFadeIn();
    }
    private void DisplayObjectives()
    {
        // task prompt
        taskCompletionPrompts.gameObject.SetActive(true);
        taskCompletionPrompts.gameObject.GetComponent<MouseTutorial>().Init();
        taskCompletionPrompts.BeginFadeIn();
        //fix  prompt
        fusePrompt.gameObject.SetActive(true);
        fusePrompt.gameObject.GetComponent<MouseTutorial>().Init();
        fusePrompt.BeginFadeIn();
    }

    private void DisableRechargePointer()
    {
        LightManager.instance.OnFullyCharged -= DisableRechargePointer;

    }

    private void RechargePrompt()
    {
        LightManager.instance.OnChargeDepleted -= RechargePrompt;
        LightManager.instance.OnFullyCharged += DisableRechargePointer;
        rechargePrompt.gameObject.SetActive(true);
        rechargePrompt.gameObject.GetComponent<MouseTutorial>().Init();
        rechargePrompt.BeginFadeIn();
        //SetUpRechargePointer();

    }
    private void BeginFightTutorial()
    {
        LevelLampsManager.instance.OnLampBroke -= BeginFightTutorial;
        ClearActivePrompts();
        FightTutorial(null);
    }
    private void FightTutorial(GameObject go)
    {

        //use gadget prompt
        gadgetPrompt.gameObject.SetActive(true);
        gadgetPrompt.gameObject.GetComponent<SingleKeyTutorial>().Init();
        gadgetPrompt.BeginFadeIn();
        //shoot weapon prompt
        shootPrompt.gameObject.SetActive(true);
        shootPrompt.gameObject.GetComponent<SingleKeyTutorial>().Init();
        shootPrompt.BeginFadeIn();

        WeaponManager.instance.OnClipEmpty += OutOfAmmoTutorial;
    }


    private void OutOfAmmoTutorial()
    {
        ClearActivePrompts();
        WeaponManager.instance.OnClipEmpty -= OutOfAmmoTutorial;
        //reload prompt
        reloadPrompt.gameObject.SetActive(true);
        reloadPrompt.gameObject.GetComponent<SingleKeyTutorial>().Init();
        reloadPrompt.BeginFadeIn();


        //weapon switch prompt
        switchWeaponPrompt.gameObject.SetActive(true);
        switchWeaponPrompt.gameObject.GetComponent<SingleKeyTutorial>().Init();
        switchWeaponPrompt.BeginFadeIn();

        
    }

    private void DecrementTutorialsLeft()
    {
        nTutorialsLeft--;
        if(nTutorialsLeft <= 0)
        {
            GameStateManager.instance.runTimeData.isTutoiralFinished = true;
            Destroy(gameObject);
        }
    }

    //private void SetUpObjectivePointer()
    //{
    //    Transform playerTransform = FindObjectOfType<PlayerBehaviour>().transform;
    //    objectivePointer = Instantiate(pointerPrefab, playerTransform.position, Quaternion.identity);
    //    objectivePointer.Init(playerTransform, FindObjectOfType<MainPowerSwitch>().transform);
    //    objectivePointer.ActivatePointer();
    //}
    //private void SetUpRechargePointer()
    //{
    //    Transform playerTransform = FindObjectOfType<PlayerBehaviour>().transform;
    //    rechargePointer = Instantiate(rechargePointerPrefab, playerTransform.position, Quaternion.identity);
    //    rechargePointer.Init(playerTransform, FindObjectOfType<RechargeStationBehaviour>().transform);
    //    rechargePointer.ActivatePointer();
    //}
    private void OnDestroy()
    {
        InitStateManager.instance.OnStateChange -= EvaluateInitState;
        GameStateManager.instance.OnGameStateChange -= EvaluateGameState;
    }

    public void ClearActivePrompts()
    {
        foreach(BaseTutorial tut in allTutorials)
        {
            if (tut.GetIsActive()) tut.DisablePrompt();
        }
    }



}
