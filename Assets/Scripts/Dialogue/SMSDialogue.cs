using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Speaker
{
    Client,
    MainCharacter
};

public enum DialogueState
{
    Idle,
    Busy
};


public class SMSDialogue : MonoBehaviour
{
    //visual display
    [SerializeField] private Image clientImage;
    [SerializeField] private Image mcImage;
    [SerializeField] private StoryData storyData;
    [SerializeField] private List<DialgoueOption> dialogueOptions;
    [SerializeField] private GameObject typingBubblePrefab;
    private GameObject typingBubble;
    //Display Settings
    [SerializeField] private Vector2 clientSmsOffset;
    [SerializeField] private Vector2 mcSmsOffset;
    [SerializeField] private Vector2 bubbleOffset;
    [SerializeField] private Vector2 typingOffset;

    [SerializeField] private Color clientBubbleColor;
    [SerializeField] private Color mcBubbleColor;

    [SerializeField] private Transform smsClientStartPosition;
    [SerializeField] private Transform smsMCStartPosition;
    [SerializeField] private Transform smsArea;
    //print states
    private Speaker currentSpeaker;
    private DialogueState currentDialogueState;

    private SMSBubble previousBubble;

    public delegate void BeatEvaluationDelegate(BeatData beat);
    public event BeatEvaluationDelegate OnBeatDisplayed;


  
  

    public void Init(Speaker currentSpeaker)
    {
      
        this.currentSpeaker = currentSpeaker;

        //If the current speaker is not the main character
        if(currentSpeaker!= Speaker.MainCharacter)
        {
            //hide dialogue Options
            foreach(DialgoueOption options in dialogueOptions)
            {
                options.gameObject.SetActive(false);
                options.OnDialogueSelected += ClearOptions;
            }
        }
        currentDialogueState = DialogueState.Idle;
        typingBubble = Instantiate(typingBubblePrefab, transform);
        typingBubble.SetActive(false);
    }


    public void DisplayBeat(int id, Speaker newSpeaker)
    {
        currentSpeaker = newSpeaker;
        BeatData data = storyData.GetBeatById(id);
        //if there is no previous bubble assume this is the start of the conversation
        //if there is no previous bubble assume this is the start of the conversation
        if (previousBubble == false)
        {
            DisplayFirstBeat(data);
        }
        //if not append from previous bubble
        else
        {
            DisplaySmsBubble(data);
        
        }

        OnBeatDisplayed?.Invoke(data);
    }

    public void DisplayChoice(int choiceIndex, string text,int targetBeatID)
    {
       
        if (choiceIndex < dialogueOptions.Count)
        {
            dialogueOptions[choiceIndex].gameObject.SetActive(true);

            dialogueOptions[choiceIndex].SetUpDialogue(text, targetBeatID);
        }
    }
    public void DisplayClientBeat(int id, float typingTime, Speaker newSpeaker)
    {
        currentSpeaker = newSpeaker;
        BeatData data = storyData.GetBeatById(id);
        StartCoroutine(DoDisplay(data, typingTime));

    }

    private IEnumerator DoDisplay(BeatData data, float typingTime)
    {
        while (IsBusy())
        {
            yield return null; //wait until dialogue is no longer busy
        }
        currentDialogueState = DialogueState.Busy;
        DisplayTypingBubble();
        //wait time should probably create a small texting bubble
        yield return new WaitForSeconds(typingTime);
        typingBubble.SetActive(false);
        //if there is no previous bubble assume this is the start of the conversation
        if (previousBubble == false)
        {
            DisplayFirstBeat(data);
        }
        //if not append from previous bubble
        else
        {

            DisplaySmsBubble(data);
        }
        currentDialogueState = DialogueState.Idle;
        OnBeatDisplayed?.Invoke(data);
    }

    public void SetCurrentSpeaker(Speaker newSpeaker)
    {
        currentSpeaker = newSpeaker;
    }


    public bool IsBusy() { return currentDialogueState != DialogueState.Idle; }

    private void ClearOptions()
    {
        foreach (DialgoueOption options in dialogueOptions)
        {
            options.gameObject.SetActive(false);
        }
    }


    private void OnDestroy()
    {
        foreach (DialgoueOption options in dialogueOptions)
        {
            options.OnDialogueSelected -= ClearOptions;
        }
    }


    private void DisplayTypingBubble()
    {
        Vector2 pos;

        typingBubble.SetActive(true);
        if (previousBubble == false)
        {
            pos = (Vector2)smsClientStartPosition.position + typingOffset;
            typingBubble.transform.position = pos + new Vector2(typingBubble.GetComponent<RectTransform>().rect.width / 2, 0f);
        }
        else
        {
            float bubbleHeight = typingBubble.gameObject.GetComponent<RectTransform>().rect.height;
            float bubbleWidth;
            pos = new Vector2(smsClientStartPosition.position.x, previousBubble.transform.position.y - bubbleHeight / 2)
                     + typingOffset + bubbleOffset;


            bubbleWidth = typingBubble.GetComponent<RectTransform>().rect.width;
            //Update position
            typingBubble.transform.position = pos + new Vector2(bubbleWidth / 2, 0f); ;
        }
           
    }

    private void DisplayFirstBeat(BeatData data)
    {
        Vector2 pos;
        SMSBubble newBubble;
        float bubbleHeight;
        float bubbleWidth;
        switch (currentSpeaker)
        {
            case Speaker.Client:
                //Create newbubble
                newBubble = DialogueManager.instance.CreateSMSBubble(smsArea);
                newBubble.SetUp(data.DisplayText, clientBubbleColor);

                //get dimension
                bubbleHeight = newBubble.GetComponent<RectTransform>().rect.height;
                bubbleWidth = newBubble.GetComponent<RectTransform>().rect.width;

                //calculate position at the top of the screen in the client case
                pos = (Vector2)smsClientStartPosition.position + clientSmsOffset;

                //update postion and display text
                newBubble.transform.position = pos + new Vector2(bubbleWidth/2,-(bubbleHeight/2));
                previousBubble = newBubble;
                break;

            //calculate position at the top of the screen in the mc case
            case Speaker.MainCharacter:
                newBubble = DialogueManager.instance.CreateSMSBubble(smsArea);
                newBubble.SetUp(data.DisplayText, mcBubbleColor);

                //get dimension
                bubbleHeight = newBubble.GetComponent<RectTransform>().rect.height;
                bubbleWidth = newBubble.GetComponent<RectTransform>().rect.width;

                //calculate position at the top of the screen in the client case
                pos = (Vector2)smsMCStartPosition.position + mcSmsOffset;

                //update postion and display text
                newBubble.transform.position = pos + new Vector2(bubbleWidth / 2, -(bubbleHeight / 2));
                previousBubble = newBubble;
                break;

        }
    }
    private void DisplaySmsBubble(BeatData data)
    {
        Vector2 pos;
        SMSBubble newBubble;
        float bubbleHeight = previousBubble.GetComponent<RectTransform>().rect.height; ;
        float newBubbleHeight;
        float newBubbleWidth;
        switch (currentSpeaker)
        {
            case Speaker.Client:
                //set up sms bubbles
                newBubble = DialogueManager.instance.CreateSMSBubble(smsArea);
                newBubble.SetUp(data.DisplayText, clientBubbleColor);

                //get bubble dimensions
                newBubbleHeight = newBubble.GetComponent<RectTransform>().rect.height;
                newBubbleWidth = newBubble.GetComponent<RectTransform>().rect.width;

                //Spawn new bubble with an offset of its previous position + the offset of it's height from its centre 
                pos = new Vector2(smsClientStartPosition.position.x+ newBubbleWidth / 2, previousBubble.transform.position.y - bubbleHeight / 2 - newBubbleHeight/2)
                    + clientSmsOffset + bubbleOffset;

                //Update position
                newBubble.transform.position = pos;
                previousBubble = newBubble;

                //Display text
                break;

            case Speaker.MainCharacter:
                //set up sms bubbles
                newBubble = DialogueManager.instance.CreateSMSBubble(smsArea);
                newBubble.SetUp(data.DisplayText, mcBubbleColor);

                //get bubble dimensions
                newBubbleHeight = newBubble.GetComponent<RectTransform>().rect.height;
                newBubbleWidth = newBubble.GetComponent<RectTransform>().rect.width;

                //Spawn new bubble with an offset of its previous position + the offset of it's height from its centre 
                pos = new Vector2(smsMCStartPosition.position.x - newBubbleWidth / 2, previousBubble.transform.position.y - bubbleHeight / 2 - newBubbleHeight / 2)
                    + mcSmsOffset + bubbleOffset;

                //Update position
                newBubble.transform.position = pos;
                previousBubble = newBubble;

                //Display text
                break;
        }
    }
}
