
using UnityEngine;
using TMPro;
public class InGamePrompt : MonoBehaviour
{
    private TextMeshPro promptText;
    private Transform playerTrans;
    public static InGamePrompt instance;
    public Vector3 offset;
    private void Awake()
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
        promptText.text = newString;
    }

    public void HidePrompt()
    {
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
}
