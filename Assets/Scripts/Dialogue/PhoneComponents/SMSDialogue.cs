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

public enum ScrollMode
{
    Up,
    Down
};



public class SMSDialogue : MonoBehaviour
{
    //visual display
    [SerializeField] private Image clientImage;
    [SerializeField] private Image mcImage;
    [SerializeField] private StoryData storyData;
    [SerializeField] private List<DialgoueOption> dialogueOptions;
    [SerializeField] private GameObject typingBubblePrefab;
    [SerializeField] private PhoneVibration vibrationController;
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
    [SerializeField] private Transform smsScreenEndPoint;
    [SerializeField] private Transform smsArea;

    [SerializeField] private List<SMSBubble> smsBubbles = new List<SMSBubble>();

    private ScrollMode scrollMode;
    private bool isScrolling=false;

    [SerializeField] private float scrollRate;

    //print states
    private Speaker currentSpeaker;
    private DialogueState currentDialogueState;

    private SMSBubble previousBubble;

    public delegate void BeatEvaluationDelegate(BeatData beat);
    public event BeatEvaluationDelegate OnBeatDisplayed;


    private void Update()
    {
        if (isScrolling&&!IsBusy()&&smsBubbles.Count>0)
        {
            ScrollText();
        }
    }


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
        typingBubble = Instantiate(typingBubblePrefab, smsArea);
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

    public void DisplayChoice(int choiceIndex, string text,int targetBeatID,ChoiceData choice)
    {
       
        if (choiceIndex < dialogueOptions.Count)
        {
            dialogueOptions[choiceIndex].gameObject.SetActive(true);

            if(choice.ChoiceResult != string.Empty)
            dialogueOptions[choiceIndex].SetUpDialogue(text, targetBeatID, choice);
            else dialogueOptions[choiceIndex].SetUpDialogue(text, targetBeatID);
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
            typingBubble.transform.position = pos + new Vector2(typingBubble.GetComponent<RectTransform>().rect.width / 2,
               - typingBubble.GetComponent<RectTransform>().rect.height / 2);
        }
        else
        {
            float bubbleHeight = typingBubble.gameObject.GetComponent<RectTransform>().rect.height;
            float prevbubbleHeight = previousBubble.gameObject.GetComponent<RectTransform>().rect.height;
            float bubbleWidth;
            pos = new Vector2(smsClientStartPosition.position.x, previousBubble.transform.position.y - prevbubbleHeight / 2 - bubbleHeight / 2)
                     + typingOffset;


            bubbleWidth = typingBubble.GetComponent<RectTransform>().rect.width;
            pos += new Vector2(bubbleWidth / 2, 0f); ;


            //Update position
            typingBubble.transform.position = pos;

            if (pos.y < smsScreenEndPoint.position.y)
            {
                float vertDistance = smsScreenEndPoint.position.y - pos.y;
                ScrollUnitsUp(vertDistance + bubbleHeight / 2);
                typingBubble.transform.position = (Vector3)new Vector2(typingBubble.transform.position.x,
                    typingBubble.transform.position.y + vertDistance + bubbleHeight / 2);
            }
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

                smsBubbles.Add(previousBubble);
                vibrationController.BeginViewBob();
                AudioManager.instance.PlayAtRandomPitch("PhoneVibrateSFX");
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
                newBubble.transform.position = pos + new Vector2(-bubbleWidth / 2, -(bubbleHeight / 2));
                previousBubble = newBubble;

                smsBubbles.Add(previousBubble);
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

                smsBubbles.Add(previousBubble);

                if (pos.y < smsScreenEndPoint.position.y)
                {
                    float vertDistance = smsScreenEndPoint.position.y - pos.y;
                    ScrollUnitsUp(vertDistance+newBubbleHeight / 2);
                }
                vibrationController.BeginViewBob();
                AudioManager.instance.PlayAtRandomPitch("PhoneVibrateSFX");

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

                smsBubbles.Add(previousBubble);
                if (pos.y < smsScreenEndPoint.position.y)
                {
                    float vertDistance = smsScreenEndPoint.position.y - pos.y;
                    ScrollUnitsUp(vertDistance+newBubbleHeight / 2);
                }

                break;
        }
    }


    public void ScrollText()
    {
        switch (scrollMode)
        {
            case ScrollMode.Down:
                Debug.Log("Scrolling down");
                if (CanScrollDown())
                {

                    ScrollDown();
                }
       
                break;
            case ScrollMode.Up:
                Debug.Log("Scrolling up");
                if (CanScrollUp())
                {
                    ScrollUp();

                }
                break;
        }
    
    }

    public void BeginScrolling(ScrollMode mode)
    {
        scrollMode = mode;
        isScrolling = true;
    }

    public void EndScrolling()
    {

        isScrolling = false;
    }
    public void ScrollUp()
    {
        foreach (SMSBubble bubble in smsBubbles)
        {
            bubble.transform.position = Vector2.Lerp(bubble.transform.position, new Vector2(bubble.transform.position.x,
                bubble.transform.position.y - 1), scrollRate * Time.deltaTime);
        }
    }
    public void ScrollUnitsUp(float scrollAmount)
    {
        foreach (SMSBubble bubble in smsBubbles)
        {
            bubble.transform.position = (Vector3)new Vector2(bubble.transform.position.x, bubble.transform.position.y + scrollAmount);
        }
    }
    public void ScrollUnitsDown(float scrollAmount)
    {
        foreach (SMSBubble bubble in smsBubbles)
        {
            bubble.transform.position += (Vector3)new Vector2(bubble.transform.position.x, scrollAmount);
        }
    }
    public void ScrollDown()
    {
        foreach (SMSBubble bubble in smsBubbles)
        {
            bubble.transform.position = Vector2.Lerp(bubble.transform.position, new Vector2(bubble.transform.position.x,
                bubble.transform.position.y + 1), scrollRate * Time.deltaTime);
        }

    }
    public bool CanScrollUp()
    {
     
        Vector2 position = (Vector2)smsBubbles[0].transform.position + new Vector2(0.0f,
            smsBubbles[0].GetComponent<RectTransform>().rect.height / 2);
        if (position.y > smsClientStartPosition.position.y)
        {
            return true;
        }
        return false;


    }

    public bool CanScrollDown()
    {
        int lastIndex = smsBubbles.Count - 1;
        Vector2 position = (Vector2)smsBubbles[lastIndex].transform.position - new Vector2(0.0f,
            smsBubbles[lastIndex].GetComponent<RectTransform>().rect.height/2);
        if (position.y < smsScreenEndPoint.position.y)
        {
            return true;
        }
        return false;
    }

    public void DisplayClientImage(Sprite portrait)
    {
        if (!clientImage.gameObject.activeSelf) clientImage.gameObject.SetActive(true);
        clientImage.sprite = portrait;
    }
}
