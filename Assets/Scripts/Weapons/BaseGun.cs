using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGun : MonoBehaviour
{
    protected GunTypes gunType;
    [Header("Gun Settings")]
    //Ammo max values
    public int maxAmmo, maxClip;
    //Ammo current
    protected int currentAmmo, currentClip;
    //Reload and shot interval max value
    public float reloadTime,maxTimeBetweenShots;
    //Current time between reload and shot interval
    protected float currentReloadTime, currentTimeBetweenShots;

    protected bool canShoot;

    [Header("Gun Shooting Settings")]
    public GameObject bulletPrefab;
    public float shotForce;
    [Range(0f,20f)]
    public float sprayRange;
    public float spray;
    [Header("Gun Components")]
    public Transform firePoint;

    [Header("Camera Shake Bariables")]
     public float time,magnitude,smoothIn,smoothOut;

    virtual protected void Awake()
    {
        //Initialise variables
        canShoot = true;
        currentClip = maxClip;
        currentAmmo = maxAmmo;
    }

    
    virtual public void Shoot()
    {
        //Shoot behaviour
        //If ammo is available and can shoot
        if (currentClip > 0 && canShoot)
        {
            CamShake.instance.DoScreenShake(time, magnitude, smoothIn, smoothOut);
            //Instatiate a bullet
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();//Get RB component

            
            if (bulletRB != null)
            {
                //Adds force to shoot bullet
                bulletRB.AddForce(firePoint.up * shotForce,ForceMode2D.Impulse);
                currentClip--;
                canShoot = false;
            }
            else
            {
                //bullet can't get shot so just destroy
                Destroy(bulletRB);
            }
        }
    }

    virtual public void Reload()
    {
        //ReloadBehaviour
    }


    //Getters
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
    public int GetCurrentClip()
    {
        return currentClip;
    }

    protected IEnumerator ShotDelay()
    {
        yield return new WaitForSeconds(maxTimeBetweenShots);
        canShoot = true;
    }
}
