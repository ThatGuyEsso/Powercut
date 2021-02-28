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
    [SerializeField] private string UIDclient;
    private Client client;
    private bool hasAlert;
    private int beat;


    public void SetUpMessage(int beat)
    {
        this.beat = beat;
        alertIcon.SetActive(true);
    }

    public void OpenContact()
    {
        if (hasAlert) TabletMenuManager.instance.StartDialogue(beat,client.ClientImage);
    }

    public void RefreshContact()
    {
        client = ClientManager.instance.GetClient(UIDclient);
        if (client != null )
        {
            hasAlert = client.hasMessage;
            if (hasAlert)
            {
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
    }


    public void OnEnable()
    {
        RefreshContact();
    }






}
