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
    [SerializeField] private GameObject typingBubble;
    //Display Settings
    [SerializeField] private Vector2 clientSmsOffset;
    [SerializeField] private Vector2 mcSmsOffset;
    [SerializeField] private Vector2 bubbleOffset;
    [SerializeField] private Vector2 typingOffset;

    [SerializeField] private Color clientBubbleColor;
    [SerializeField] private Color mcBubbleColor;

    [SerializeField] private Transform smsClientStartPosition;
    [SerializeField] private Transform smsMCStartPosition;
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
            Vector2 pos;
            switch (currentSpeaker)
            {
                case Speaker.Client:
                    //calculate position at the top of the screen in the client case
                    pos = (Vector2)smsClientStartPosition.position + clientSmsOffset;
                    previousBubble = DialogueManager.instance.CreateSMSBubble(transform);

                    //update postion and display text
                    previousBubble.SetUp(data.DisplayText, Color.white);
                    previousBubble.transform.position = pos + new Vector2(previousBubble.GetComponent<RectTransform>().rect.width / 2, 0f);

                    break;

                //calculate position at the top of the screen in the mc case
                case Speaker.MainCharacter:
                    pos = (Vector2)smsMCStartPosition.position + mcSmsOffset;
                    previousBubble = DialogueManager.instance.CreateSMSBubble(transform);


                    //update postion and display text
                    previousBubble.SetUp(data.DisplayText, Color.green);
                    previousBubble.transform.position = pos - new Vector2(previousBubble.GetComponent<RectTransform>().rect.width / 2, 0f); ;
                    break;
            }
        }
        //if not append from previous bubble
        else
        {
            Vector2 pos;
            float bubbleHeight = previousBubble.gameObject.GetComponent<RectTransform>().rect.height;
            float bubbleWidth;
            switch (currentSpeaker)
            {
                case Speaker.Client:
                    //Spawn new bubble with an offset of its previous position + the offset of it's height from its centre 
                    pos = new Vector2(smsClientStartPosition.position.x, previousBubble.transform.position.y - bubbleHeight / 2)
                        + clientSmsOffset + bubbleOffset;
                    //create new bubble and parent it to the canvas
                    previousBubble = DialogueManager.instance.CreateSMSBubble(transform);
                    previousBubble.SetUp(data.DisplayText, clientBubbleColor);
                    bubbleWidth = previousBubble.gameObject.GetComponent<RectTransform>().rect.width;
                    //Update position
                    previousBubble.transform.position = pos + new Vector2(bubbleWidth / 2, 0f); ;
                    //Display text
                    break;

                case Speaker.MainCharacter:
                    pos = new Vector2(smsMCStartPosition.position.x, previousBubble.transform.position.y - bubbleHeight / 2)
                  + mcSmsOffset + bubbleOffset;
                    //create new bubble and parent it to the canvas
                    previousBubble = DialogueManager.instance.CreateSMSBubble(transform);
                    previousBubble.SetUp(data.DisplayText, mcBubbleColor);

                    bubbleWidth = previousBubble.gameObject.GetComponent<RectTransform>().rect.width;
                    //caluclate new bubble offset of its previous position + the offset of it's height from its centre
                    //Update position
                    previousBubble.transform.position = pos - new Vector2(bubbleWidth / 2, 0f); ; ;
                    //Display text
                    break;
            }
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
            Vector2 pos;
            switch (currentSpeaker)
            {
                case Speaker.Client:
                    //calculate position at the top of the screen in the client case
                    pos = (Vector2)smsClientStartPosition.position + clientSmsOffset;
                    previousBubble = DialogueManager.instance.CreateSMSBubble(transform);

                    //update postion and display text
                    previousBubble.SetUp(data.DisplayText, clientBubbleColor);
                    previousBubble.transform.position = pos + new Vector2(previousBubble.GetComponent<RectTransform>().rect.width / 2, 0f);

                    break;

                //calculate position at the top of the screen in the mc case
                case Speaker.MainCharacter:
                    pos = (Vector2)smsMCStartPosition.position + mcSmsOffset;
                    previousBubble = DialogueManager.instance.CreateSMSBubble(transform);


                    //update postion and display text
                    previousBubble.SetUp(data.DisplayText, mcBubbleColor);
                    previousBubble.transform.position = pos - new Vector2(previousBubble.GetComponent<RectTransform>().rect.width / 2, 0f); ;
                    break;
            }
        }
        //if not append from previous bubble
        else
        {
            Vector2 pos;
            float bubbleHeight = previousBubble.gameObject.GetComponent<RectTransform>().rect.height;
            float bubbleWidth;
            switch (currentSpeaker)
            {
                case Speaker.Client:
                    //Spawn new bubble with an offset of its previous position + the offset of it's height from its centre 
                    pos = new Vector2(smsClientStartPosition.position.x , previousBubble.transform.position.y - bubbleHeight / 2)
                        + clientSmsOffset + bubbleOffset;
                    //create new bubble and parent it to the canvas
                    previousBubble = DialogueManager.instance.CreateSMSBubble(transform);
                    previousBubble.SetUp(data.DisplayText,clientBubbleColor);
                     bubbleWidth = previousBubble.gameObject.GetComponent<RectTransform>().rect.width;
                    //Update position
                    previousBubble.transform.position = pos + new Vector2(bubbleWidth / 2, 0f); ;
                    //Display text
                    break;

                case Speaker.MainCharacter:
                    pos = new Vector2(smsMCStartPosition.position.x , previousBubble.transform.position.y - bubbleHeight / 2)
                  + mcSmsOffset + bubbleOffset;
                    //create new bubble and parent it to the canvas
                    previousBubble = DialogueManager.instance.CreateSMSBubble(transform);
                    previousBubble.SetUp(data.DisplayText, mcBubbleColor);

                    bubbleWidth = previousBubble.gameObject.GetComponent<RectTransform>().rect.width;
                    //caluclate new bubble offset of its previous position + the offset of it's height from its centre
                    //Update position
                    previousBubble.transform.position = pos - new Vector2(bubbleWidth / 2, 0f); ; ;
                    //Display text
                    break;
            }
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
}
