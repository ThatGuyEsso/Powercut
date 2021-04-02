using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoBehaviour,IInitialisable
{
    public List<Client> clients = new List<Client>();
    private SceneIndex previousLevelScene;
    public static ClientManager instance;
    private Client activeClient;
    public void BindToInitManager()
    {
        InitStateManager.instance.OnStateChange += EvaluateNewState;
        InitStateManager.instance.OnContinue  += LoadClientSaves;
    }
 
    private void AddClientsToSaveData()
    {
        foreach (Client client in clients)
        {
            ContactData data = new ContactData(client.ClientID);

        }
    }
    private void LoadClientSaves()
    {
        foreach (Client client in clients)
        {
           ContactData data= SaveData.current.LoadContactData(client.ClientID);
            if (data != null)
            {
                client.unlocked = data.isUnlocked;
            }
        }
    }
    private void Awake()
    {
        previousLevelScene = SaveData.current.lastSession.lastLevel;
        AddClientsToSaveData();
    }

    private void EvaluateNewState(InitStates newState)
    {
        switch (newState)
        {
            case InitStates.LevelClear:
                previousLevelScene = TransitionManager.instance.GetCurrentLevel();
                break;
            case InitStates.MainMenu:
                EvaluateMessages();
                break;

        }
    }

    private void EvaluateMessages()
    {
        bool hasMessages = false;
        foreach (Client client in clients)
        {
            if (client.PointToNewBeat(previousLevelScene)) hasMessages=true;
        }

        if (hasMessages) TabletMenuManager.instance.ToggleSmsAlert();
    }
   

    public void SetUpClients()
    {
        foreach(Client client in clients)
        {
            client.ClearMessage();
            for (int i =0; i< client.LevelTriggers.Count; i++)
            {
                if(i < client.Beats.Count)
                {
                    client.BindToResult(client.LevelTriggers[i], client.Beats[i]);
                }
                else
                {
                    Debug.LogError(" triggers out of range of Beats");
                }
            }

            if (client.unlocked && client.ClientID == "FF") client.unlocked = false;


            if (client.unlocked && client.ClientID == "NCI") client.unlocked = false;

        }
  
    }
    public void Init()
    {
        if (instance == false)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SetUpClients();
        BindToInitManager();
    }

    public Client GetClient(string UID)
    {
        return clients.Find(client => client.ClientID == UID);
    }

    public void SetActiveClient(Client client)
    {
        activeClient = client;
    }
    public Client GetActiveClient() { return activeClient; }
    private void OnDestroy()
    {
        InitStateManager.instance.OnContinue  -= LoadClientSaves;
        InitStateManager.instance.OnStateChange -= EvaluateNewState;
    }
}
