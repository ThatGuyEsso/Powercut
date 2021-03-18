using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneAnimEventListener : MonoBehaviour
{
    private Animator anim;

    public delegate void PhoneAnimDelegate();
    public event PhoneAnimDelegate phoneShown;
    public event PhoneAnimDelegate phoneHidden;
    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    public void OnPopUpAnimationEnd()
    {
     
 
        phoneShown?.Invoke();
    }

    public void OnPopDownAnimationEnd()
    {

        phoneHidden?.Invoke();
    }

    public void OnPlayButtonAnimEnd()
    {

        anim.enabled = false;
    }
}
