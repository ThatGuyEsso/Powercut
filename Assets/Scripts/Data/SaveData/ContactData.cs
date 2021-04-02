using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ContactData 
{
    public string ID;
    public bool isUnlocked;

    public ContactData(string id)
    {
        ID = id;
    }
    public ContactData()
    {
    }

}
