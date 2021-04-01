using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidePhone : MonoBehaviour
{
    [SerializeField] private Animator phoneAnim;
    [SerializeField] private bool isActive=true;
    [SerializeField] private bool bindToHiddenEvent;
    [SerializeField] private PhoneButtonWidget Widget;
    public void BeginHidePhone()
    {
        if (isActive)
        {
            phoneAnim.enabled = true;
            phoneAnim.Play("PhonePopDown");
            AudioManager.instance.PlayAtRandomPitch("PhonePullOutSFX");
            AudioManager.instance.PlayRandFromGroup("PhoneButtonSFX");
            if (bindToHiddenEvent)
            {
                phoneAnim.gameObject.GetComponent<PhoneAnimEventListener>().phoneHidden += Hidden;
            }

        }
        else
        {
            Widget.UpdateLabel("Can't do that right now", Color.red);
        }
 
    }


    private void Hidden() {
        phoneAnim.enabled = false;
        phoneAnim.gameObject.GetComponent<PhoneAnimEventListener>().phoneHidden -= Hidden;
    }
}
