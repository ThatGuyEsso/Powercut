using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    //static instance
    public static DialogueManager instance;

    //Contains all the results we want to process
    [SerializeField] private ResultData resultData;
    [SerializeField] private StoryData beatData;


    [SerializeField] private SMSBubble bubblePrefab;
    [SerializeField] private SMSDialogue dialogueMenu;


    //beat to send to
    public static int nextBeat;

    private void Awake()
    {
        if (instance == false)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
        dialogueMenu = FindObjectOfType<SMSDialogue>();
        dialogueMenu.OnBeatDisplayed += EvaluateBeat;
        dialogueMenu.Init(Speaker.Client);
    }

    private void Start()
    {
        dialogueMenu.DisplayBeat(1,Speaker.Client);
    }
    public SMSBubble CreateSMSBubble(Transform parent)
    {
        SMSBubble bubble = Instantiate(bubblePrefab, parent);
        return bubble;
    }


    public void EvaluateBeat(BeatData beat)
    {
     
        if (beat.IsEnd)
        {

        }
        //if beat doesn't end dialogue
        //evaluate next dialogue beats
        else
        {
            //if dialogue has choices
            if (beat.GetChoices().Count > 0){
                //Get all choices
                List< ChoiceData > choices = beat.GetChoices();

                //generate a choice display for each choice
                for (int i =0; i < beat.GetChoices().Count; i++)
                {
                    dialogueMenu.DisplayChoice(i, choices[i].DisplayText, choices[i].NextID);
                }
            }
            else
            {
             
                if (beat.Chains)
                {
                    if (beatData.GetBeatById(beat.TargetID).IsClientBeat)
                    {
                        dialogueMenu.DisplayClientBeat(beat.TargetID, beatData.GetBeatById(beat.TargetID).TypeTime,Speaker.Client);
                    }
                    else
                    {
                        dialogueMenu.DisplayClientBeat(beat.TargetID, beatData.GetBeatById(beat.TargetID).TypeTime,Speaker.MainCharacter);
                    }
                }
            
               
            }
         
        }
    }

    public void DisplayDialogueChoice(int id)
    {
        dialogueMenu.DisplayBeat(id, Speaker.MainCharacter);
    }


    public void OnDestroy()
    {
        dialogueMenu.OnBeatDisplayed -= EvaluateBeat;
    }

}
