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
    [SerializeField] private Animator phoneAnim;
    [SerializeField] private PhoneAnimEventListener animEvents;
    //beat to send to
    public static int nextBeat;
    //beat to send to
    public static Speaker nextSpeaker;
    //beat to send to
    public static Sprite currentClientPortrait;

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


        dialogueMenu.OnBeatDisplayed += EvaluateBeat;
        dialogueMenu.Init(Speaker.Client);

        if (phoneAnim != false)
        {
            animEvents.phoneHidden += PhoneScreenHidden; 
            animEvents.phoneShown += DisplayBeat;
        }
   

        if (dialogueMenu.gameObject.activeSelf)
            dialogueMenu.gameObject.SetActive(false);

    }


    public SMSBubble CreateSMSBubble(Transform parent)
    {
        SMSBubble bubble = Instantiate(bubblePrefab, parent);
        return bubble;
    }

    public Result GetResultByName(string name)
    {
        return resultData.GetConsequenceByName(name);
    }

    public void EvaluateBeat(BeatData beat)
    {
     
        if (beat.IsEnd)
        {
            phoneAnim.enabled = true;
            phoneAnim.Play("ActivatePlayButton");
            if (beat.HasScene)
            {
                FindObjectOfType<PhoneButtons>().EnableStartButton(beat.TargetScene);
            }
            FindObjectOfType<PhoneButtons>().EnableStartButton();
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
                    dialogueMenu.DisplayChoice(i, choices[i].DisplayText, choices[i].NextID,choices[i]);
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

    public void DisplayBeat(int id, Speaker speaker)
    {
        dialogueMenu.DisplayBeat(id, speaker);
    }


    public void DisplayBeat()
    {

        phoneAnim.enabled = false;
        if(dialogueMenu.gameObject.activeSelf)
            dialogueMenu.DisplayBeat(nextBeat, nextSpeaker);
    }

    public void SetUpNextBeat(int id, Speaker speaker)
    {
        nextBeat = id;
        nextSpeaker = speaker;
  
    }

    public void SetUpPortrait(Sprite portrait)
    {
        dialogueMenu.DisplayClientImage(portrait);
    }
    public void OnDestroy()
    {
        dialogueMenu.OnBeatDisplayed -= EvaluateBeat;
        animEvents.phoneHidden -= DisplayBeat;
        animEvents.phoneShown -= PhoneScreenHidden;
    }

    public void ToggleDialogueScreen(bool isShown, bool isAnimated)
    {
        if (isShown)
        {
            InitStateManager.currGameMode = GameModes.Dialogue;
            if (currentClientPortrait != false) dialogueMenu.DisplayClientImage(currentClientPortrait);
        }
        PlayerBehaviour player;
        if ((player = FindObjectOfType<PlayerBehaviour>()) != false&& isShown)
            player.EnableControls();
        
   
        if (isAnimated && isShown)
        {
            dialogueMenu.gameObject.SetActive(true);
            phoneAnim.enabled = true;
            phoneAnim.Play("PhonePopUP");
            AudioManager.instance.PlayAtRandomPitch("PhonePullOutSFX");
        }
        else if(isAnimated && !isShown){
            phoneAnim.enabled = true;
            phoneAnim.Play("PhonePopDown");
            AudioManager.instance.PlayAtRandomPitch("PhonePullOutSFX");
        }
        else {

            DisplayBeat();
        }
    }


    public void PhoneScreenHidden()
    {
        PlayerBehaviour player;
        if ((player= FindObjectOfType < PlayerBehaviour>()) != false)
            player.EnableControls();

        phoneAnim.enabled = false;
        dialogueMenu.gameObject.SetActive(false);
        InitStateManager.currGameMode = GameModes.Powercut;

    }

 
   
}
