using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunTypes
{
    Pistol,
    Shotgun,
    AutoRifle
}
public class WeaponManager : MonoBehaviour
{

    public static WeaponManager instance;
    public Shotgun shotgun;
    private GunTypes activeGun;
    private GunTypes[] gunsCarried = new GunTypes[2];
    private void Awake()
    {
        //Create static instance of this class
        instance = this;
        shotgun = FindObjectOfType<Shotgun>();
    }


    public void SetActiveWeapon(GunTypes newGun)
    {
        activeGun = newGun;
    }
    
   public void ShootActiveWeapon()
    {
        switch (activeGun)
        {
            case GunTypes.Shotgun:
                shotgun.Shoot();
                break;
        }
    }


}
