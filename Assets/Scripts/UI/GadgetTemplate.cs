using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GadgetTemplate : MonoBehaviour
{
    private Image gadgetIcon;
    private TextMeshProUGUI counter;
    private int currCounter;
    public GadgetTypes type;
    private void Awake()
    {
        GetChildReferences();
    }


    private void GetChildReferences()
    {
        gadgetIcon = transform.Find("GadgetImage").GetComponent<Image>();
        counter = transform.Find("GadgetAmount").GetComponent<TextMeshProUGUI>();
    }



    public void SetUpTemplate(Sprite icon, int amount,GadgetTypes typeOfGadget)
    {
        currCounter = amount;
        type = typeOfGadget;
        gadgetIcon.sprite = icon;
        counter.text = currCounter.ToString();
    }

    public void IncrementCounter()
    {
        currCounter++;
        counter.text = currCounter.ToString();

    }
    public void DecrementCounter()
    {
        currCounter--;
        if (currCounter < 0)
        {
            currCounter = 0;
        }
        counter.text = currCounter.ToString();

    }
}
