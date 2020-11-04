using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages current state of level so component knows what face were in (I.E Player Death, Game completed, objectives complete)

public enum GameStates
{
    MainPowerOn,
    MainPowerOff,
    TasksCompleted
};
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;
    private  GameStates currentGameState;
    private void Awake()
    {
        
        if (instance == false)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }



    //Starts the game
    public void SwitchPowerOff()
    {
        currentGameState = GameStates.MainPowerOff;
        Debug.Log("Power is off");
    }

    public void SwitchPowerOn()
    {
        currentGameState = GameStates.MainPowerOn;
        Debug.Log("Power is off");
    }

    public GameStates GetCurrentGameState()
    {
        return currentGameState;
    }
}
