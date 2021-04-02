using System.Collections;


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
