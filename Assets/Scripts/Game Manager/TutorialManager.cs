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
    [SerializeField] private List<BaseTutorial> allTutorials;

    [SerializeField] private float timebeforeTutorialStart =1.5f;
    [SerializeField] private int nTutorialsLeft;
    bool gameplayTutorialTriggered =false;


    public void Init()
    {

        nTutorialsLeft = allTutorials.Count;

        foreach (BaseTutorial tutorial in allTutorials)
        {
            tutorial.OnTutorialComplete += DecrementTutorialsLeft;
        }

        GameStateManager.instance.OnGameStateChange += EvaluateGameState;
        Invoke("OrientationTutorial", timebeforeTutorialStart);



   
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
                }
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


    private void OnDestroy()
    {

        GameStateManager.instance.OnGameStateChange -= EvaluateGameState;
    }
}
