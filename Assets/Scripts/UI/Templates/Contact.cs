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

    private bool hasAlert;
    private int beat;


    public void SetUpMessage(int beat)
    {
        this.beat = beat;
        hasAlert = true;
        alertIcon.SetActive(true);
    }


    public void RefreshContact()
    {
        if (hasAlert) alertIcon.SetActive(false);
    }

  

}
