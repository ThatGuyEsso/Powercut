using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{

    public SaveData()
    {
        clientsData = new List<ContactData>();
        lastSession = new SessionData();
        soundSettings = new SoundSettingData();
    }
    public static SaveData _current;
    public static SaveData current
    {
        get
        {
            if (_current == null)
            {
                _current = new SaveData();
               
            }
            return _current;
        }

        set
        {
            _current = value;
        }
    }

    public SessionData lastSession;
 
    public List<ContactData> clientsData;

    public SoundSettingData soundSettings;
     

    public void SaveContactData( bool unlocked, string clientID)
    {
        if(clientsData==null) clientsData = new List<ContactData>();
        //Is there any client save data
        if (clientsData.Count <= 0)
        {
            //if not create and a new contact and add to the data
            ContactData contactData = new ContactData(clientID);
            contactData.isUnlocked = unlocked;

            clientsData.Add(contactData);
        }
        else
        {
            //There are already contact entries so look through them for the correct ID
            ContactData contact =null;//stores data if contact is found

            for(int i=0; i< clientsData.Count; i++)
            {
                //If data has a match
                if (clientID == clientsData[i].ID)
                {
                    //set contact to that and break loop
                    contact = clientsData[i];
                    break;
                }
            }

            //contact is not null so it was found
            if (contact != null)
            {
                //update its data
                contact.isUnlocked = unlocked;

            }
            //if contact is null no exisiting contact was found
            else
            {
                //create a new contact entry and store it
                contact = new ContactData(clientID);
                contact.isUnlocked = unlocked;
                clientsData.Add(contact);
            }
        }
    }


    public ContactData LoadContactData(string clientID)
    {
        if (clientsData == null) clientsData = new List<ContactData>();
        //if there are existing entries
        if (clientsData.Count > 0)
        {
            for (int i = 0; i < clientsData.Count; i++)
            {
                //If data has a match
                if (clientID == clientsData[i].ID)
                {
                    //return the contact data
                    return clientsData[i];
                }
            }

     
        }
        //if it did return at this point the client couldn't be found or the list is empty so return null
        return null;
    }
    public void ClearSave()
    {
        lastSession.Reset();
        if(clientsData!=null)
            clientsData.Clear();
   
    }

}
