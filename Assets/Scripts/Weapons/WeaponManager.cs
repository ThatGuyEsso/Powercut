using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunTypes
{
    Pistol,
    Shotgun,
    AutoRifle
};

public enum GadgetTypes
{
    FlashBang
};
public class WeaponManager : MonoBehaviour
{

    public static WeaponManager instance;
    public Shotgun shotgun;
    public Pistol pistol;
    private GunTypes activeGun;
    private GadgetTypes primaryGadget;
    private GadgetTypes secondaryGadget;
    public GameObject flashBangPrefab;
    public float throwForce;
 
    private void Awake()
    {
        //Create static instance of this class
        instance = this;
        shotgun = FindObjectOfType<Shotgun>();
        pistol = FindObjectOfType<Pistol>();
        UIManager.instance.gunDisplay.UpdateActiveGun(shotgun.gunPortrait, pistol.gunPortrait);
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
            switch (activeGun)
            {
                case GunTypes.Shotgun:

                    UIManager.instance.gunDisplay.UpdateActiveGun(shotgun.gunPortrait,pistol.gunPortrait);

                    break;

                case GunTypes.Pistol:
                    UIManager.instance.gunDisplay.UpdateActiveGun(pistol.gunPortrait, shotgun.gunPortrait);

                    break;
            }
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

    public void SetUpGadget(GadgetTypes[] gadgets,int primaryAmount, int secondaryAmount)
    {
        //if there is a secondary and primary gadget
        if (gadgets.Length > 1 && gadgets.Length < 3)
        {
            primaryGadget = gadgets[0];
            secondaryGadget= gadgets[1];
            UIManager.instance.gadgetDisplay.GenerateNewGadgetTemplate(primaryGadget, primaryAmount);
            //UIManager.instance.gadgetDisplay.GenerateNewGadgetTemplate(secondaryGadget, secondaryAmount);
        }

        else
        {
            primaryGadget = gadgets[0];
            UIManager.instance.gadgetDisplay.GenerateNewGadgetTemplate(primaryGadget, primaryAmount);
            Debug.Log("Spawn gadgets");
        }
    }


    public void UsePrimaryGadget(int newAmount, Vector2 dir,Vector3 origin)
    {
        switch (primaryGadget)
        {
            case GadgetTypes.FlashBang:
                FlashBang flashBang = Instantiate(flashBangPrefab, origin, Quaternion.identity).GetComponent<FlashBang>();
                flashBang.LaunchFlashBang(dir, throwForce);
                UIManager.instance.gadgetDisplay.DecreaseRespectiveGadgetCounter(primaryGadget);
                break;
        }
    }

    public GunTypes GetActiveGun()
    {
        return activeGun;
    }

    public void GainPrimaryGadget()
    {

    }


    public void UseSecondaryGadget()
    {

    }
    public void GainSecondaryGadget()
    {

    }

}
