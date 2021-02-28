using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : BaseGun
{
    override protected void Awake()
    {
        base.Awake();
        //set appropriate gun type
        gunType = GunTypes.Pistol;



    }

    override public void Reload()
    {
       //if not reloading
        if ( !isReloading )
        {
            AudioManager.instance.PlayAtRandomPitch(reloadSFX);
            //if there are no bullets in the chamber
            if (currentClip <= 0)
            {


                //No bullets so give full clip
                isReloading = true;
                StartCoroutine(ReloadRoutine(maxClip));
               

            }
            else//still bullets left so
            {
                //get the remaining to fill chamber
                int clipSlotsleft = maxClip - currentClip;
                isReloading = true;
                //reload with that amount
                StartCoroutine(ReloadRoutine(clipSlotsleft));
             
            }
        }
        //ReloadBehaviour
    }
}
