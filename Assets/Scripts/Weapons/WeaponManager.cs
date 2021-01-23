using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GadgetTypes
{
    FlashBang
};
public class WeaponManager : MonoBehaviour
{

    public static WeaponManager instance;

    //stores number of guns carried
    private List<BaseGun> gunsCarried = new List<BaseGun>();
    //stores refrence to active gun in list of guns
    private int activeGunIndex =0;

    private GadgetTypes primaryGadget;
    private GadgetTypes secondaryGadget;
    public GameObject flashBangPrefab;
    public float throwForce;

    public void Awake()
    {
        if (instance == false)
        {
            instance = this;

        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Init()
    {

        SetGunAmmoDisplay();
        UIManager.instance.gunDisplay.UpdateActiveGun(gunsCarried[activeGunIndex].gunPortrait, gunsCarried[DetermineSecondaryGun()].gunPortrait);

    }

    public void SwitchWeapons( )
    {
        if (!IsActiveGunReloading())
        {
            //Increment index
            activeGunIndex++;
            //If index reaches end of array
            if (activeGunIndex >= gunsCarried.Count)
            {
                activeGunIndex = 0;//loop back round
            }
            SetGunAmmoDisplay();
            //Update the portraits
            UIManager.instance.gunDisplay.UpdateActiveGun(gunsCarried[activeGunIndex].gunPortrait, gunsCarried[DetermineSecondaryGun()].gunPortrait);

        }
    }
    
   public void ShootActiveWeapon()
    {
        //gets currently active gun and shoots them
        gunsCarried[activeGunIndex].Shoot();
        UIManager.instance.ammoDisplay.SetClipCount(gunsCarried[activeGunIndex].GetCurrentClip());
   }

    public void ReloadActiveWeapon()
    {
        //gets currently active gun and reloadsit
        gunsCarried[activeGunIndex].Reload();

    }

    public void SetGunAmmoDisplay()
    {
        //sets ammo display of respective gun
        UIManager.instance.ammoDisplay.SetAmmoCount(gunsCarried[activeGunIndex].GetCurrentAmmo(), gunsCarried[activeGunIndex].HasInifiniteBullets());
        UIManager.instance.ammoDisplay.SetClipCount(gunsCarried[activeGunIndex].GetCurrentClip());

    }
    public void UpdateGunClipDisplay()
    {

        //updates thee clips only of respectve gun
        UIManager.instance.ammoDisplay.SetClipCount(gunsCarried[activeGunIndex].GetCurrentClip());
 
    }
    public void UpdateGunAmmoDisplay()
    {
        //updates  only ammo reserve display of respective gun
        UIManager.instance.ammoDisplay.SetAmmoCount(gunsCarried[activeGunIndex].GetCurrentAmmo(), gunsCarried[activeGunIndex].HasInifiniteBullets());

    }

    public bool IsActiveGunReloading()
    {
       //checks if currently equipped gun is reloading
        return gunsCarried[activeGunIndex].GetIsReloading();

    }

    public void SetUpGadget(GadgetTypes[] gadgets,int primaryAmount, int secondaryAmount)
    {
        //if there is a secondary and primary gadget
        if (gadgets.Length > 1 && gadgets.Length < 3)
        {
            //assign gadgets
            primaryGadget = gadgets[0];
            secondaryGadget = gadgets[1];
            //Update UI
            UIManager.instance.gadgetDisplay.GenerateNewGadgetTemplate(primaryGadget, primaryAmount);
        }
        else
        {
            //assign gadgets
            primaryGadget = gadgets[0];
            //Update UI
            UIManager.instance.gadgetDisplay.GenerateNewGadgetTemplate(primaryGadget, primaryAmount);

        }
    }


    public void UsePrimaryGadget(int newAmount, Vector2 dir,Vector3 origin)
    {
        //Checks primarily assign gadget and uses them
        switch (primaryGadget)
        {
            case GadgetTypes.FlashBang:
                FlashBang flashBang = Instantiate(flashBangPrefab, origin, Quaternion.identity).GetComponent<FlashBang>();
                flashBang.LaunchFlashBang(dir, throwForce);
                UIManager.instance.gadgetDisplay.DecreaseRespectiveGadgetCounter(primaryGadget);
                break;
        }
    }

    //loops through all guns carried and resets them
    private void ResetGuns()
    {
        foreach(BaseGun gun in gunsCarried)
        {
            gun.ResetGun();
        }

    }

    public BaseGun GetActiveGun()
    {
        //return active gun
        return gunsCarried[activeGunIndex];
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
    public void BindToInitManager()
    {
        InitStateManager.instance.OnStateChange += EvaluateNewState;
    }
    private void EvaluateNewState(InitStates newState)
    {
        switch (newState)
        {
            case InitStates.RespawnPlayer:
                ResetGuns();
                break;
            //case InitStates.SpawnPlayer:
            //    Init();
            //    break;
        }
    }

    //takes an array of all guns carried and adds the to guns carried array
    public void InitEquippedGuns(BaseGun[] guns)
    {
        for(int i=0; i< guns.Length; i++)
        {
            //player should only ever have 2 guns
            if (i < 2) gunsCarried.Add(guns[i]); //in that case add current gun to list
        }
    }

    private int DetermineSecondaryGun()
    {
        //check if gun index overflows
        if(activeGunIndex +1 >= gunsCarried.Count)
        {
            //if it does then it must the last index, so return 0
            return 0;
        }
        else
        {
            // if not then it must be the first so return the next index value
            return activeGunIndex + 1;
        }
    }
}
