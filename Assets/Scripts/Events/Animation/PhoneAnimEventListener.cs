using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneAnimEventListener : MonoBehaviour
{


    public void OnPopUpAnimationEnd()
    {
        DialogueManager.instance.DisplayBeat();
       
    }

    public void OnPopDownAnimationEnd()
    {
        DialogueManager.instance.PhoneScreenHidden();

    }
}
