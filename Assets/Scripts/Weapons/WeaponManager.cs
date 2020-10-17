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
    public Pistol pistol;
    private GunTypes activeGun;

    private void Awake()
    {
        //Create static instance of this class
        instance = this;
        shotgun = FindObjectOfType<Shotgun>();
        pistol = FindObjectOfType<Pistol>();
    }

    private void Start()
    {
        SetGunAmmoDisplay();
    }
    public void SetActiveWeapon(GunTypes newGun)
    {
        if (!IsActiveGunReloading())
        {

            activeGun = newGun;
            SetGunAmmoDisplay();
        }
    }
    
   public void ShootActiveWeapon()
    {
        switch (activeGun)
        {
            case GunTypes.Shotgun:
                shotgun.Shoot();
                UIManager.instance.ammoDisplay.SetClipCount(shotgun.GetCurrentClip());
                break;

            case GunTypes.Pistol:
                pistol.Shoot();
                UIManager.instance.ammoDisplay.SetClipCount(pistol.GetCurrentClip());
                break;
        }
   }

    public void ReloadActiveWeapon()
    {
        switch (activeGun)
        {
            case GunTypes.Shotgun:
                shotgun.Reload();

                break;
            case GunTypes.Pistol:
                pistol.Reload();
                break;
        }

    }

    public void SetGunAmmoDisplay()
    {
        switch (activeGun)
        {
            case GunTypes.Shotgun:
                UIManager.instance.ammoDisplay.SetAmmoCount(shotgun.GetCurrentAmmo());
                UIManager.instance.ammoDisplay.SetClipCount(shotgun.GetCurrentClip());
                break;

            case GunTypes.Pistol:
                UIManager.instance.ammoDisplay.SetAmmoCount(pistol.GetCurrentAmmo());
                UIManager.instance.ammoDisplay.SetClipCount(pistol.GetCurrentClip());
                break;
        }
    }
    public void UpdateGunClipDisplay()
    {
        switch (activeGun)
        {
            case GunTypes.Shotgun:
                
                UIManager.instance.ammoDisplay.SetClipCount(shotgun.GetCurrentClip());
                break;

            case GunTypes.Pistol:
            
                UIManager.instance.ammoDisplay.SetClipCount(pistol.GetCurrentClip());
                break;
        }
    }
    public void UpdateGunAmmoDisplay()
    {
        switch (activeGun)
        {
            case GunTypes.Shotgun:
                UIManager.instance.ammoDisplay.SetAmmoCount(shotgun.GetCurrentAmmo());
              
                break;

            case GunTypes.Pistol:

                UIManager.instance.ammoDisplay.SetClipCount(pistol.GetCurrentAmmo());
                break;
        }
    }

    public bool IsActiveGunReloading()
    {
        switch (activeGun)
        {

            case GunTypes.Shotgun:
                return shotgun.GetIsReloading();

            case GunTypes.Pistol:
                return pistol.GetIsReloading();

            default:
                return false;

        }
    }

}
