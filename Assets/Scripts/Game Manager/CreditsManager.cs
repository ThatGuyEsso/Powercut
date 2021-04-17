using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsManager : MonoBehaviour
{
    [SerializeField] private bool playOneAwake;
    [SerializeField] private Credits credits;
    [SerializeField] private float creditWaitTime;
    [SerializeField] private float authorWaitTime;
    [SerializeField] private Animator animController;
    [SerializeField] private PhoneAnimEventListener animListener;
    [SerializeField] private GameObject phone;
    private ScrollMode scrollMode;
    private bool isScrolling = false;
    [SerializeField] private float scrollRate;
    [SerializeField] private List<Transform> smsBubbles = new List<Transform>();
    [SerializeField] private Transform creditsStartPos;
    [SerializeField] private Transform authorStartPos;
    [SerializeField] private Transform screenEndPoint;
    [SerializeField] private Transform smsArea;
    [SerializeField] private GameObject typingBubblePrefab;
    [SerializeField] private PhoneVibration vibrationController;
    [SerializeField] private SMSBubble bubblePrefab;
    [SerializeField] private Vector2 authorBubbleOffset;
    [SerializeField] private Vector2 creditBubbleOffSet;
    [SerializeField] private Vector2 bubbleOffset;
    [SerializeField] private Vector2 authorTypingOffset;
    [SerializeField] private Vector2 creditTypingOffset;
    private GameObject typingBubble;
    int currentCredit =0;
    private SMSBubble previousBubble;

    DialogueState printState;

 
    public void BeginToDisplayCredits()
    {
        if(currentCredit< credits.credit.Count)
        {
            CreditData credit = credits.credit[currentCredit];
            StartCoroutine(DoDisplayCredit(credit));
        }
     
    }



    private void Update()
    {
        if (isScrolling && !IsBusy() && smsBubbles.Count > 0)
        {
            ScrollText();
        }
    }

    public void Awake()
    {
        phone.SetActive(false);
        if (playOneAwake)
        {
            InitStateManager.instance.OnStateChange += CreditsMenuOpened;
        }

    }
    private void Init()
    {
        currentCredit = 0;
        if(!typingBubble)
            typingBubble = Instantiate(typingBubblePrefab, smsArea);
        smsBubbles.Add(typingBubble.transform);
        typingBubble.SetActive(false);
        InstantDisplay();
        BeginToDisplayCredits();
    }

    private void DisplayFirstCredit(bool isCredit, CreditData newCredit)
    {
        Vector2 pos;
        SMSBubble newBubble;
        float bubbleHeight;
        float bubbleWidth;

        if (isCredit)
        {

            //Create newbubble
            newBubble = CreateCreditBubble();
            newBubble.SetUp(newCredit.CreditText, credits.creditColour);

            //get dimension
            bubbleHeight = newBubble.GetComponent<RectTransform>().rect.height;
            bubbleWidth = newBubble.GetComponent<RectTransform>().rect.width;

            //calculate position at the top of the screen in the client case
            pos = (Vector2)creditsStartPos.position + (creditBubbleOffSet * EssoUtility.GetAspectRatio());

            //update postion and display text
            newBubble.transform.position = pos + new Vector2(bubbleWidth / 2, -(bubbleHeight / 2) * EssoUtility.GetAspectRatio());
            previousBubble = newBubble;


            smsBubbles.Add(previousBubble.transform);
            



        }
        else
        {
            newBubble = CreateCreditBubble();
            newBubble.SetUp(newCredit.CreditText, credits.authorColour);

            //get dimension
            bubbleHeight = newBubble.GetComponent<RectTransform>().rect.height;
            bubbleWidth = newBubble.GetComponent<RectTransform>().rect.width;

            //calculate position at the top of the screen in the client case
            pos = (Vector2)authorStartPos.position + (authorBubbleOffset * EssoUtility.GetAspectRatio());

            //update postion and display text
            newBubble.transform.position = pos + new Vector2(-bubbleWidth / 2, -(bubbleHeight / 2) * EssoUtility.GetAspectRatio());
            previousBubble = newBubble;

            smsBubbles.Add(previousBubble.transform);
        }
        vibrationController.BeginViewBob();
        //AudioManager.instance.PlayAtRandomPitch("PhoneVibrateSFX");
    }
    public void InstantDisplay()
    {
        CreditData credit = credits.credit[currentCredit];
        DisplayFirstCredit(credit.IsCredit, credit);

        currentCredit++;
    }

    private void CreditsMenuOpened(InitStates newState)
    {
        if (newState == InitStates.Credits)
            Invoke("ShowPhone", 1f);
          
    }
    private IEnumerator DoDisplayCredit(CreditData newCredit)
    {

        printState = DialogueState.Busy;
        DisplayTypingBubble(newCredit.IsCredit);
        //wait time should probably create a small texting bubble
        yield return new WaitForSeconds(newCredit.TypeTime);
        typingBubble.SetActive(false);
        DisplayCredit(newCredit.IsCredit, newCredit);




        printState = DialogueState.Idle;
        currentCredit++;
        BeginToDisplayCredits();
    }
    public void DisplayCredit(bool isCredit, CreditData newCredit)
    {
        Vector2 pos;
        SMSBubble newBubble;
        float bubbleHeight = previousBubble.GetComponent<RectTransform>().rect.height; ;
        float newBubbleHeight;
        float newBubbleWidth;
        if(isCredit)
        {

            //set up sms bubbles
                newBubble = CreateCreditBubble();
                newBubble.SetUp(newCredit.CreditText, credits.creditColour);

                //get bubble dimensions
                newBubbleHeight = newBubble.GetComponent<RectTransform>().rect.height;
                newBubbleWidth = newBubble.GetComponent<RectTransform>().rect.width;

                //Spawn new bubble with an offset of its previous position + the offset of it's height from its centre 
                pos = new Vector2(creditsStartPos.position.x + newBubbleWidth / 2, previousBubble.transform.position.y - bubbleHeight / 2 - newBubbleHeight / 2)
                    + (creditBubbleOffSet + bubbleOffset) * EssoUtility.GetAspectRatio();

                //Update position
                newBubble.transform.position = pos;
                previousBubble = newBubble;

                smsBubbles.Add(previousBubble.transform);

                if (pos.y < screenEndPoint.position.y)
                {
                    float vertDistance = screenEndPoint.position.y - pos.y;
                    ScrollUnitsUp(vertDistance + newBubbleHeight / 2);
                }
    

  


        }
        else
        {
            //set up sms bubbles
            newBubble = CreateCreditBubble();
            newBubble.SetUp(newCredit.CreditText, credits.authorColour);

            //get bubble dimensions
            newBubbleHeight = newBubble.GetComponent<RectTransform>().rect.height;
            newBubbleWidth = newBubble.GetComponent<RectTransform>().rect.width;

            //Spawn new bubble with an offset of its previous position + the offset of it's height from its centre 
            pos = new Vector2(authorStartPos.position.x - newBubbleWidth / 2, previousBubble.transform.position.y - bubbleHeight / 2 - newBubbleHeight / 2)
                + (authorBubbleOffset + bubbleOffset) * EssoUtility.GetAspectRatio();

            //Update position
            newBubble.transform.position = pos;
            previousBubble = newBubble;

            smsBubbles.Add(previousBubble.transform);
            if (pos.y < screenEndPoint.position.y)
            {
                float vertDistance = screenEndPoint.position.y - pos.y;
                ScrollUnitsUp(vertDistance + newBubbleHeight / 2);
            }
        }
        vibrationController.BeginViewBob();
        AudioManager.instance.PlayAtRandomPitch("PhoneVibrateSFX");
    }
    public void DisplayTypingBubble(bool isCredit)
    {
        Vector2 pos;
        
        if (isCredit)
        {

            typingBubble.SetActive(true);
            if (previousBubble == false)
            {
                pos = (Vector2)creditsStartPos.position + creditTypingOffset;
                typingBubble.transform.position = pos + new Vector2(typingBubble.GetComponent<RectTransform>().rect.width / 2,
                   -typingBubble.GetComponent<RectTransform>().rect.height / 2);
            }
            else
            {
                float bubbleHeight = typingBubble.gameObject.GetComponent<RectTransform>().rect.height;
                float prevbubbleHeight = previousBubble.gameObject.GetComponent<RectTransform>().rect.height;
                float bubbleWidth;
                pos = new Vector2(creditsStartPos.position.x, previousBubble.transform.position.y - prevbubbleHeight / 2 - bubbleHeight / 2)
                         + creditTypingOffset;


                bubbleWidth = typingBubble.GetComponent<RectTransform>().rect.width;
                pos += new Vector2(bubbleWidth / 2, 0f); ;


                //Update position
                typingBubble.transform.position = pos;

                if (pos.y < screenEndPoint.position.y)
                {
                    float vertDistance = screenEndPoint.position.y - pos.y;
                    ScrollUnitsUp(vertDistance + bubbleHeight / 2);
                    typingBubble.transform.position = (Vector3)new Vector2(typingBubble.transform.position.x,
                        typingBubble.transform.position.y + vertDistance + bubbleHeight / 2);
                }
            }
        }
        else
        {
            typingBubble.SetActive(true);
            if (previousBubble == false)
            {
                pos = (Vector2)authorStartPos.position + authorTypingOffset;
                typingBubble.transform.position = pos + new Vector2(-typingBubble.GetComponent<RectTransform>().rect.width / 2,
                   -typingBubble.GetComponent<RectTransform>().rect.height / 2);
            }
            else
            {
                float bubbleHeight = typingBubble.gameObject.GetComponent<RectTransform>().rect.height;
                float prevbubbleHeight = previousBubble.gameObject.GetComponent<RectTransform>().rect.height;
                float bubbleWidth;
                pos = new Vector2(authorStartPos.position.x, previousBubble.transform.position.y - prevbubbleHeight / 2 - bubbleHeight / 2)
                         + authorTypingOffset;


                bubbleWidth = typingBubble.GetComponent<RectTransform>().rect.width;
                pos += new Vector2(-bubbleWidth / 2, 0f); ;


                //Update position
                typingBubble.transform.position = pos;

                if (pos.y < screenEndPoint.position.y)
                {
                    float vertDistance = screenEndPoint.position.y - pos.y;
                    ScrollUnitsUp(vertDistance + bubbleHeight / 2);
                    typingBubble.transform.position = (Vector3)new Vector2(typingBubble.transform.position.x,
                        typingBubble.transform.position.y + vertDistance + bubbleHeight / 2);
                }
            }
        }
        vibrationController.BeginViewBob();
        AudioManager.instance.PlayAtRandomPitch("PhoneVibrateSFX");
    }
    public void ScrollUp()
    {
        foreach (Transform bubble in smsBubbles)
        {
            bubble.transform.position = Vector2.Lerp(bubble.transform.position, new Vector2(bubble.transform.position.x,
                bubble.transform.position.y - 1), scrollRate * Time.deltaTime);
        }
    }
    public void ScrollUnitsUp(float scrollAmount)
    {
        foreach (Transform bubble in smsBubbles)
        {
            bubble.position = (Vector3)new Vector2(bubble.transform.position.x, bubble.transform.position.y + scrollAmount);
        }
    }
    public void ScrollUnitsDown(float scrollAmount)
    {
        foreach (Transform bubble in smsBubbles)
        {
            bubble.position += (Vector3)new Vector2(bubble.transform.position.x, scrollAmount);
        }
    }
    public void ScrollDown()
    {
        foreach (Transform bubble in smsBubbles)
        {
            bubble.position = Vector2.Lerp(bubble.transform.position, new Vector2(bubble.transform.position.x,
                bubble.position.y + 1), scrollRate * Time.deltaTime);
        }

    }
    public bool CanScrollUp()
    {

        Vector2 position = (Vector2)smsBubbles[0].transform.position + new Vector2(0.0f,
            smsBubbles[0].GetComponent<RectTransform>().rect.height / 2);
        if (position.y > authorStartPos.position.y)
        {
            return true;
        }
        return false;


    }

    public bool CanScrollDown()
    {
        int lastIndex = smsBubbles.Count - 1;
        Vector2 position = (Vector2)smsBubbles[lastIndex].transform.position - new Vector2(0.0f,
            smsBubbles[lastIndex].GetComponent<RectTransform>().rect.height / 2);
        if (position.y < screenEndPoint.position.y)
        {
            return true;
        }
        return false;
    }
    private SMSBubble CreateCreditBubble()
    {
        SMSBubble bubble = Instantiate(bubblePrefab, smsArea);
        return bubble;
    }
    public bool IsBusy() { return printState != DialogueState.Idle; }
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

    public void PhoneHidden()
    {
        ClearBubbles();
        animListener.phoneHidden -= PhoneHidden;
        phone.SetActive(false);
        animController.enabled = false;
    }

    public void PhoneVisible()
    {
        Init();
        animController.enabled = false;
        animListener.phoneShown -= PhoneVisible;
    }

    public void HidePhone()
    {
        animController.enabled = true;
        animListener.phoneHidden += PhoneHidden;
        animController.Play("PhonePopDown");
        AudioManager.instance.PlayRandFromGroup("PhoneButtonSFX");
        AudioManager.instance.PlayAtRandomPitch("PhonePullOutSFX");
        StopAllCoroutines();

    }

    public void ToTitle()
    {
      
        AudioManager.instance.PlayRandFromGroup("PhoneButtonSFX");

        TransitionManager.instance.ReturnToTitleScreen();
      
    }

    public void ShowPhone()
    {
        if (!phone.activeSelf)
        {
            phone.SetActive(true);
            animController.enabled = true;
            animListener.phoneShown += PhoneVisible;
            animController.Play("PhonePopUP");
            AudioManager.instance.PlayAtRandomPitch("PhonePullOutSFX");
        }
    
    }

    public void ClearBubbles()
    {
        foreach(Transform bubble in smsBubbles)
        {
            Destroy(bubble.gameObject);
        }
        smsBubbles.Clear();
    }

}

