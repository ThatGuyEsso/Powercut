using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsMenuScroll : MonoBehaviour
{
    [SerializeField] private CreditsManager creditsManager;
    [SerializeField] private ScrollMode mode;
    public void StartScrolling()
    {
        creditsManager.BeginScrolling(mode);
    }

    public void StopScrolling()
    {
        creditsManager.EndScrolling();
    }
}
