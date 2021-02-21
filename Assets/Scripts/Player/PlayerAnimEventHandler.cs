using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEventHandler : MonoBehaviour
{
    [SerializeField]
    private string stepSFX;
    public void OnStrideFinished()
    {
        AudioManager.instance.PlayRandFromGroup(stepSFX);
    }
}
