using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunTypes
{
    Pistol,
    Shotgun,
    AutoRifle
};

public abstract class BaseGun : MonoBehaviour, IShootable
{
    protected GunTypes gunType;
    [Header("Gun Settings")]
    //Ammo max values
    [Tooltip("If max ammo is less than zero game assumes gun has maximum ammo")]
    [SerializeField] protected int maxAmmo, maxClip;
    //Ammo current
    protected int currentAmmo, currentClip;
    public Sprite gunPortrait;
    [SerializeField]
    protected string shootSFX;
    [SerializeField]
    protected string reloadSFX;
    [SerializeField]
    protected GameObject muzzleFlashSFX;
    protected MuzzleFlash muzzleFlash;
    [SerializeField]
    protected float flashDuration =0.2f;
    //Reload and shot interval max value
    public float reloadTime,maxTimeBetweenShots;
    //Current time between reload and shot interval
    protected float currentReloadTime, currentTimeBetweenShots;

    protected bool canShoot;
    protected bool isReloading;

    [Header("Gun Shooting Settings")]
    public GameObject bulletPrefab;
    public float shotForce;
    public float knockBack;
    public float minDamage;
    public float maxDamage;
    [Range(0f,90f)]
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
        isReloading = false;
    
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
            GameObject bullet = ObjectPoolManager.Spawn(bulletPrefab, firePoint.position, firePoint.rotation);
            IShootable shot = bullet.GetComponent<IShootable>();
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();//Get RB component

            
            if (bulletRB != null)
            {
                BeginMuzzleVFX();
                AudioManager.instance.PlayAtRandomPitch(shootSFX);
                //Adds force to shoot bullet
                float dmg = Random.Range(minDamage, maxDamage);
                bulletRB.AddForce(firePoint.up * shotForce,ForceMode2D.Impulse);
                shot.SetUpBullet(knockBack, dmg);
                currentClip--;
                canShoot = false;
            }
            else
            {
                //bullet can't get shot so just destroy
                ObjectPoolManager.Recycle(bulletRB);
            }
            StartCoroutine(ShotDelay());
        }
        else if(currentClip<= 0)
        {
            AudioManager.instance.PlayAtRandomPitch("OutOfAmmoSFX");
        }
    }

    virtual public void Reload()
    {
        if(currentAmmo > 0 && !isReloading && currentClip <maxClip)
        {
            AudioManager.instance.PlayAtRandomPitch(reloadSFX);
            if (currentClip <= 0)
            {
                if (currentAmmo >= maxClip)
                {
                    isReloading = true;
                    StartCoroutine(ReloadRoutine(maxClip));
                }
                else
                {
                    isReloading = true;
                    int difference = maxClip - currentAmmo;
                    StartCoroutine(ReloadRoutine(difference));
                }

            }
            else
            {
                int clipSlotsleft = maxClip - currentClip;

                if(currentAmmo>= currentClip)
                {
                    isReloading = true;
                    StartCoroutine(ReloadRoutine(clipSlotsleft));
                }
                else
                {
                    isReloading = true;
                    int difference = clipSlotsleft - currentAmmo;
                    StartCoroutine(ReloadRoutine(difference));
                }
            }
        }
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
    public bool GetIsReloading()
    {
        return isReloading;
    }
    protected IEnumerator ShotDelay()
    {
        yield return new WaitForSeconds(maxTimeBetweenShots);
        canShoot = true;


    }

    virtual protected IEnumerator ReloadRoutine(int newClip)
    {
     
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
      
        currentAmmo -= newClip;
        if (HasInifiniteBullets()) currentAmmo = maxAmmo;
        currentClip += newClip;
     

        UIManager.instance.ammoDisplay.SetAmmoCount(currentAmmo, HasInifiniteBullets());
      
        UIManager.instance.ammoDisplay.SetClipCount(currentClip);
    }

    protected void BeginMuzzleVFX()
    {
        if(muzzleFlash == false)
        {
            muzzleFlash = Instantiate(muzzleFlashSFX, firePoint.position, firePoint.rotation).GetComponent<MuzzleFlash>();
            muzzleFlash.transform.SetParent(firePoint);
            muzzleFlash.Init();
        }

        muzzleFlash.BeginMuzzleFash(flashDuration, firePoint);
    }
    void IShootable.SetUpBullet(float knockBack, float damage)
    {

    }

    public bool HasInifiniteBullets()
    {
        //if true the gun has infinite bullets
        return maxAmmo < 0;
    }
    //Reset gun
    public void ResetGun()
    {
        currentClip = maxClip;
        currentAmmo = maxAmmo;
    }

    public GunTypes GetGunTypes()
    {
        //returns type of the gun
        return gunType;
    }
}
