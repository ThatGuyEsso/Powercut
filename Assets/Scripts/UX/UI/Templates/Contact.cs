using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Contact : MonoBehaviour
{
    [SerializeField] private Image contactImage;
    [SerializeField] private TextMeshProUGUI contactName;
    [SerializeField] private GameObject alertIcon;
    [SerializeField] private Image background;
    [SerializeField] private string UIDclient;
    private Client client;
    private bool hasAlert=false;
    private int beat;
    [SerializeField] private bool isVisible;


    public void SetUpMessage(int beat)
    {
        this.beat = beat;
        alertIcon.SetActive(true);
    }

    public void OpenContact()
    {
        AudioManager.instance.PlayRandFromGroup("PhoneButtonSFX");
        if (hasAlert)
        {
            TabletMenuManager.instance.StartDialogue(beat, client.ClientImage);
            ClientManager.instance.SetActiveClient(client);
        }
    }

    public void RefreshContact()
    {
  
        client = ClientManager.instance.GetClient(UIDclient);
        if (client != null )
        {
            hasAlert = client.hasMessage;
            if (hasAlert)
            {
                isVisible = hasAlert;
                ToggleVisibility(isVisible);
                SetUpMessage(client.DialogueBeat);
                
            }
            else
            {
                alertIcon.SetActive(false);
            }
        }
        else
        {
            alertIcon.SetActive(false);
        }
        if (!isVisible)
        {
            ToggleVisibility(isVisible);

        }
    }


    public void OnEnable()
    {
        RefreshContact();
    }


    public void ToggleVisibility(bool isVisible)
    {
        contactImage.gameObject.SetActive(isVisible);
        contactName.enabled = isVisible;
        alertIcon.SetActive( isVisible);
        background.enabled = isVisible;
    }



}
