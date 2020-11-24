using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActiveGunDisplay : MonoBehaviour
{
    public Image activeGun;
    public Image holsteredGun;


    public void UpdateActiveGun(Sprite newActiveGun, Sprite newHolsteredGun)
    {
        activeGun.sprite = newActiveGun;
        UpdateHolsteredGun(newHolsteredGun);

    }

    public void UpdateHolsteredGun(Sprite newHolsteredGun)
    {
        holsteredGun.sprite = newHolsteredGun;

    }


}
