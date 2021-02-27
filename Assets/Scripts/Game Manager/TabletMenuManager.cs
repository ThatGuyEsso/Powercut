using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TabletMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject smsMenu;
    [SerializeField] private GameObject contactsMenu;

    GraphicRaycaster raycaster;
    private void Awake()
    {
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

}
