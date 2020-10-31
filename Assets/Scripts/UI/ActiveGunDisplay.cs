using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ActiveGunDisplay : MonoBehaviour
{
    public Image primaryGun;
    public Image secondaryGun;

    public float inactiveOppacity;
    public void DisplayPrimaryGun()
    {
        primaryGun.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        secondaryGun.color = new Color(1.0f, 1.0f, 1.0f, inactiveOppacity);

    }

    public void DisplaySecondaryGun()
    {
        secondaryGun.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        primaryGun.color = new Color(1.0f, 1.0f, 1.0f, inactiveOppacity);

    }


}
