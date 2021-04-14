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
            if (client.HasConversation())
            {
                TabletMenuManager.instance.ResumeDialogue(client.GetConversation(), client.ClientImage);
                ClientManager.instance.SetActiveClient(client);
            }
            else
            {
                DialogueManager.instance.SetClient(client);
                ClientManager.instance.SetActiveClient(client);
                TabletMenuManager.instance.StartDialogue(beat, client.ClientImage);
            }
       
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
             
                client.unlocked = true;
                alertIcon.SetActive(hasAlert);
                ToggleVisibility(client.unlocked);
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
    
        ToggleVisibility(client.unlocked);

  

    }


    public void OnEnable()
    {
        RefreshContact();
    }


    public void ToggleVisibility(bool isVisible)
    {
        contactImage.gameObject.SetActive(isVisible);
        contactName.enabled = isVisible;
 
        background.enabled = isVisible;

        SaveData.current.SaveContactData(isVisible, client.ClientID);
  
    }



}
