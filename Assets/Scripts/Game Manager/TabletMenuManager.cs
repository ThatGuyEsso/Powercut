using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TabletMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject smsMenu;
    [SerializeField] private SmsButton smsButton;
    [SerializeField] private GameObject contactsMenu;
    public static TabletMenuManager instance;

    GraphicRaycaster raycaster;
    private void Awake()
    {
        if (instance == false)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        raycaster = gameObject.GetComponent<GraphicRaycaster>();
        if (contactsMenu.activeSelf) contactsMenu.SetActive(false);
        if (smsMenu.activeSelf) smsMenu.SetActive(false);
    }


    public void OpenContacts()
    {
        raycaster.enabled = false;
        contactsMenu.SetActive(true);
        contactsMenu.GetComponent<Animator>().Play("PhonePopUP");

    }

    public void ToggleSmsAlert()
    {
        smsButton.ToggleAlert();
    }


    public void StartDialogue(int beatID, Sprite image)
    {
        contactsMenu.SetActive(false);
        smsMenu.SetActive(true);
        DialogueManager.instance.SetUpPortrait(image);
        DialogueManager.instance.SetUpNextBeat(beatID, Speaker.Client);
        DialogueManager.instance.DisplayBeat();
    }
}
