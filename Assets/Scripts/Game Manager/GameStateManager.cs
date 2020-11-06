using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages current state of level so component knows what face were in (I.E Player Death, Game completed, objectives complete)

public enum GameStates
{
    MainPowerOn,
    MainPowerOff,
    TasksCompleted,
    LevelClear
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
        Debug.Log(currentGameState.ToString());
    }
    public void TasksCompleted()
    {
        currentGameState = GameStates.TasksCompleted;
        Debug.Log("Power is off but tasks completed");
        Debug.Log(currentGameState.ToString());
    }

    public void LevelCleared()
    {
        currentGameState = GameStates.LevelClear;
        Debug.Log("level cleared");
        Debug.Log(currentGameState.ToString());
    }
    public void SwitchPowerOn()
    {
        currentGameState = GameStates.MainPowerOn;
        Debug.Log("Power is On");
        Debug.Log(currentGameState.ToString());
    }

    public GameStates GetCurrentGameState()
    {
        return currentGameState;
    }

   

}
