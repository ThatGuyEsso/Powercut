using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GadgetTemplate : MonoBehaviour
{
    private Image gadgetIcon;
    public GadgetCounter counter;

    public GadgetTypes type;
    private void Awake()
    {
        GetChildReferences();
    }


    private void GetChildReferences()
    {
        gadgetIcon = transform.Find("GadgetImage").GetComponent<Image>();
        counter = gameObject.GetComponentInChildren<GadgetCounter>();
    }


    public void ResetDisplay()
    {
        counter.ResetCounter();
    }
    public void SetUpTemplate(Sprite icon, int amount,GadgetTypes typeOfGadget)
    {
        counter.SetUpCounter(amount);
        type = typeOfGadget;
        gadgetIcon.sprite = icon;

    }


}
