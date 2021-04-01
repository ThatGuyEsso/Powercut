using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    GameModes prevMode;
    Animator phoneAnim;
    PhoneAnimEventListener animEvents;
    public static SettingsMenu instance;

    [SerializeField] private GameObject phone;
    private void Awake()
    {
        if (instance == false)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }

        phoneAnim =gameObject.GetComponent<Animator>();
        animEvents = gameObject.GetComponent<PhoneAnimEventListener>();
        if (phoneAnim != false)
        {
            animEvents.phoneHidden += ReturnToPrevGameMode;
           
        }
        
    }



    public void HideMenu()
    {
        phoneAnim.enabled = true;
        phoneAnim.Play("PhonePopDown");
        AudioManager.instance.PlayAtRandomPitch("PhonePullOutSFX");
    }
    public void ToggleSettings(bool isShown, bool isAnimated)
    {
        phone.SetActive(isShown);
        if (isShown)
        {
            prevMode = InitStateManager.currGameMode;
            if(prevMode != GameModes.Menu)
                InitStateManager.currGameMode = GameModes.Menu;

        }


        if (isAnimated && isShown)
        {
         
            phoneAnim.enabled = true;
            phoneAnim.Play("PhonePopUP");
            AudioManager.instance.PlayAtRandomPitch("PhonePullOutSFX");
        }
        else if (isAnimated && !isShown)
        {
            phoneAnim.enabled = true;
            phoneAnim.Play("PhonePopDown");
            AudioManager.instance.PlayAtRandomPitch("PhonePullOutSFX");
        }
  
    }


    private void ReturnToPrevGameMode()
    {
        InitStateManager.currGameMode = prevMode;
        phone.SetActive(false);
    }


    private void OnDestroy()
    {
        animEvents.phoneHidden -= ReturnToPrevGameMode;
    }


    public bool IsVisible()
    {
        return phone.activeSelf;
    }
}
