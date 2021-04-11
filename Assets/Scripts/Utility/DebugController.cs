using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour { 
    private bool showConsole;
    private Controls inputAction;
    private GameModes prevGameMode;

    private string input = string.Empty;

    public static DebugCommand CLEAR_LEVEL;
    public static DebugCommand Kill_PlAYER;
    public static DebugCommand DEFEAT_BOSS;
    public List<object> commandList;
    private void Awake()
    {
        inputAction = new Controls();
        inputAction.Console.Enable();
        inputAction.Console.ToggleConsole.performed += ToggleConsole;
        inputAction.Console.Return.performed += OnReturn;
        CLEAR_LEVEL = new DebugCommand("/Clear_Level", "Clears currently active level","/Clear_Level", () =>
         GameStateManager.instance.ClearLevel());

        Kill_PlAYER = new DebugCommand("/Kill_Player", "Kills player if they exist","/Kill_Player",() => KillPlayer());

        DEFEAT_BOSS = new DebugCommand("/Defeat_Boss", "Kills Boss if it exist", "/Kill_Player", () => KillBoss());
        commandList = new List<object>
        {
            CLEAR_LEVEL,
            Kill_PlAYER,
            DEFEAT_BOSS
        };
    }

    public void ToggleConsole(InputAction.CallbackContext context)
    {

        if(context.performed) showConsole = !showConsole;
        if (showConsole)
        {
            prevGameMode = InitStateManager.currGameMode;
            InitStateManager.currGameMode = GameModes.Debug;
        }
        else
        {
            InitStateManager.currGameMode = prevGameMode;
        }
    }

    private void OnGUI()
    {
        if (!showConsole) return;

        float y=0;
        GUI.Box(new Rect(0, y, Screen.width, 40), "");
        GUI.backgroundColor = new Color(1.0f, 1.0f, 1.0f, 0.6f);
        input = GUI.TextField(new Rect(10f, y + 5f, Screen.width - 20f, 30f), input);
    
    }

    public void OnReturn(InputAction.CallbackContext context)
    {
        if (context.performed&&showConsole)
        {
            HandleInput();
            input = string.Empty;
            showConsole = false;
            InitStateManager.currGameMode = prevGameMode;

        }

    }
    public void HandleInput()
    {
       
        for(int i=0; i < commandList.Count; i++)
        {
            DebugCommandBase commandBase = commandList[i] as DebugCommandBase;

            if (input.Contains(commandBase.CommandID))
            {
                if(commandList[i] as DebugCommand != null)
                {
                    (commandList[i] as DebugCommand).CallCommand();
                }
            }
        }
    }

    public void KillPlayer()
    {
        PlayerBehaviour player;
        if((player=FindObjectOfType<PlayerBehaviour>())!= false)
        {
            player.PlayerDie();
        }
    }
    private void KillBoss()
    {
        BroodNest boss = FindObjectOfType<BroodNest>();
        if (boss)
            boss.BossDefeated();
    }
    private void OnDestroy()
    {
        inputAction.Console.ToggleConsole.performed -= ToggleConsole;
        inputAction.Console.Return.performed -= OnReturn;
    }

}
