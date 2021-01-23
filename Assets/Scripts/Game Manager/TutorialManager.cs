using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private FadeMediaGroup movementPrompt;
    [SerializeField] private FadeMediaGroup rotatePrompt;
    [SerializeField] private List<BaseTutorial> allTutorials;

    [SerializeField] private float timebeforeTutorialStart =1.5f;
    [SerializeField] private int nTutorialsLeft;
    public void Init()
    {

        nTutorialsLeft = allTutorials.Count;

        foreach (BaseTutorial tutorial in allTutorials)
        {
            tutorial.OnTutorialComplete += DecrementTutorialsLeft;
        }
        Invoke("StartTutorial",timebeforeTutorialStart);

   
    }



    private void StartTutorial()
    {
        movementPrompt.gameObject.SetActive(true);
        movementPrompt.gameObject.GetComponent<MovementTutorial>().Init();
        movementPrompt.BeginFadeIn();
        rotatePrompt.gameObject.SetActive(true);
        rotatePrompt.gameObject.GetComponent<MouseTutorial>().Init();
        rotatePrompt.BeginFadeIn();
    }

    private void DecrementTutorialsLeft()
    {
        nTutorialsLeft--;
        if(nTutorialsLeft <= 0)
        {
            Destroy(gameObject);
        }
    }
}
