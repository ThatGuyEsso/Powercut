using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroll : MonoBehaviour
{
    [SerializeField] private SMSDialogue dialogueMenu;
    [SerializeField] private ScrollMode mode;
    public void StartScrolling()
    {
        dialogueMenu.BeginScrolling(mode);
    }

    public void StopScrolling()
    {
        dialogueMenu.EndScrolling();
    }
}
