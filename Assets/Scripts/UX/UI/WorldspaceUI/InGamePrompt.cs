
using UnityEngine;
using System.Collections;

using TMPro;
public class InGamePrompt : MonoBehaviour, IInitialisable
{
    private TextMeshPro promptText;
    private Transform playerTrans;
    public static InGamePrompt instance;
    public Vector3 offset;
    private void Init()
    {
        instance = this;
        promptText = gameObject.GetComponent<TextMeshPro>();
        playerTrans = FindObjectOfType<PlayerBehaviour>().transform;
        HidePrompt();
    }
    private void Update()
    {
        FollowPlayer();
    }


    public void ChangePrompt(string newString)
    {
        ShowPrompt();
        promptText.text = newString;
    }
    public void SetColor(Color newColor)
    {
        promptText.color = newColor;
    }
    public void ShowPromptTimer(string newString,float timeToHide)
    {
        StopAllCoroutines();
        promptText.text = newString;
        StartCoroutine(HidePromptDelay(timeToHide));
    }

    public void HidePrompt()
    {
        SetColor(Color.white);
        promptText.enabled = false;
    }
    public void ShowPrompt()
    {
        promptText.enabled = true;
    }

    private void FollowPlayer()
    {
        if(playerTrans != null&& promptText.enabled)
        {

            transform.position = playerTrans.position + offset;
        }
    }
    void IInitialisable.Init()
    {
        Init();
    }

    private IEnumerator HidePromptDelay(float time)
    {
        yield return new WaitForSeconds(time);
        HidePrompt();
    }
}
