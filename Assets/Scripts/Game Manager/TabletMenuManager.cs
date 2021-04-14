using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TabletMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject smsMenu;
    [SerializeField] private SmsButton smsButton;
    [SerializeField] private GameObject contactsMenu;
    [SerializeField] private CreditsManager creditManager;
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

        PointerManager.instance.SwitchToPointer();
    }


    public void OpenContacts()
    {
        raycaster.enabled = false;
        contactsMenu.SetActive(true);
        contactsMenu.GetComponent<Animator>().enabled=true;
        contactsMenu.GetComponent<Animator>().Play("PhonePopUP");
        AudioManager.instance.PlayAtRandomPitch("PhonePullOutSFX");
        AudioManager.instance.PlayRandFromGroup("PhoneButtonSFX");
        contactsMenu.GetComponent<PhoneAnimEventListener>().phoneHidden += ContactsClosed;

    }
    public void ContactsClosed()
    {
        raycaster.enabled = true;
        contactsMenu.SetActive(false);
        contactsMenu.GetComponent<PhoneAnimEventListener>().phoneHidden -= ContactsClosed;

    }

    public void TitleScreeen()
    {
        AudioManager.instance.PlayRandFromGroup("PhoneButtonSFX");

        TransitionManager.instance.ReturnToTitleScreen();
    }

    public void OpenSettings()
    {
        raycaster.enabled = false;
        SettingsMenu.instance.ToggleSettings(true, true);
        SettingsMenu.instance.gameObject.GetComponent<PhoneAnimEventListener>().phoneHidden += HideSettings;
        AudioManager.instance.PlayRandFromGroup("PhoneButtonSFX");
        AudioManager.instance.PlayAtRandomPitch("PhonePullOutSFX");
    }
    public void HideSettings()
    {
        raycaster.enabled = true;
        SettingsMenu.instance.gameObject.GetComponent<PhoneAnimEventListener>().phoneHidden -= HideSettings;
        AudioManager.instance.PlayRandFromGroup("PhoneButtonSFX");
        AudioManager.instance.PlayAtRandomPitch("PhonePullOutSFX");
    }

    public void ReturnToContacts()
    {
        raycaster.enabled = false;
        contactsMenu.SetActive(true);
        smsMenu.SetActive(false);
        AudioManager.instance.PlayRandFromGroup("PhoneButtonSFX");

    }


    public void ToggleSmsAlert()
    {
        smsButton.ToggleAlert();
        AudioManager.instance.Play("MailSFX");
    }


    public void StartDialogue(int beatID, Sprite image)
    {
        contactsMenu.SetActive(false);
        smsMenu.SetActive(true);
        DialogueManager.instance.SetUpPortrait(image);
        DialogueManager.instance.SetUpNextBeat(beatID, Speaker.Client);
        DialogueManager.instance.DisplayBeat();
    }


    public void ResumeDialogue(List<BeatData> beat, Sprite image)
    {
        contactsMenu.SetActive(false);
        smsMenu.SetActive(true);
        DialogueManager.instance.SetUpPortrait(image);
        DialogueManager.instance.ResumeConversation(beat);
      
    }
    public void Credits()
    {
        creditManager.ShowPhone();
        AudioManager.instance.PlayRandFromGroup("PhoneButtonSFX");

    }

    public void Quit()
    {
        AudioManager.instance.PlayRandFromGroup("PhoneButtonSFX");
        Application.Quit();
    }

}
