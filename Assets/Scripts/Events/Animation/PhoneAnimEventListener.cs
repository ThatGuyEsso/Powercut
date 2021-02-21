using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneAnimEventListener : MonoBehaviour
{
    private Animator anim;

    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public void OnPopUpAnimationEnd()
    {
        DialogueManager.instance.DisplayBeat();
       
    }

    public void OnPopDownAnimationEnd()
    {
        DialogueManager.instance.PhoneScreenHidden();

    }

    public void OnPlayButtonAnimEnd()
    {

        anim.enabled = false;
    }
}
