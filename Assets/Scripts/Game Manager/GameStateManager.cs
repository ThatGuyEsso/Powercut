using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Manages current state of game (I.E Player Death, paused, Game completed, objectives complete)
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance;

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





}
