using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoDisplay : MonoBehaviour
{
    private TextMeshProUGUI clipDisplay;
    private TextMeshProUGUI ammoDisplay;
    private TextMeshProUGUI separator;

    public Color noAmmoColor;
    public Color ammoAvailColor;
    private void Awake()
    {
        clipDisplay = transform.Find("clipDisplay").GetComponent<TextMeshProUGUI>();
        ammoDisplay = transform.Find("AmmoDisplay").GetComponent<TextMeshProUGUI>();
        separator = transform.Find("Separator").GetComponent<TextMeshProUGUI>();
    }



    public void SetClipCount(int newCount)
    {
        if(newCount > 0)
        {
            clipDisplay.text = newCount.ToString();
            SetCanShootColor(true); 
        }
        else{
            clipDisplay.text = "00";
            SetCanShootColor(false);
        }
    }


    public void SetAmmoCount(int newCount)
    {
        if (newCount > 0)
        {
            ammoDisplay.text = newCount.ToString();
        }
        else
        {
            clipDisplay.text = "00";
        }
    }


    public void SetCanShootColor(bool canShoot)
    {
        if (canShoot)
        {
            clipDisplay.color = ammoAvailColor;
            ammoDisplay.color = ammoAvailColor;
            separator.color = ammoAvailColor;
        }
        else
        {
            clipDisplay.color = noAmmoColor;
            ammoDisplay.color = noAmmoColor;
            separator.color = noAmmoColor;
        }
    }
}
