using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmsButton : MonoBehaviour
{
    [SerializeField] private GameObject alert;

    public void ToggleAlert()
    {
        if (alert.activeSelf) alert.SetActive(false);
        else alert.SetActive(true);
    }

}
