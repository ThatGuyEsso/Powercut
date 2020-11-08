using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : BaseGun
{
    public int bulletsPerShot;

    public override void Shoot()
    {
 
        if(currentClip>0 && canShoot)
        {
            CamShake.instance.DoScreenShake(time, magnitude, smoothIn, smoothOut);
            GameObject[] bullets = new GameObject[bulletsPerShot];
            Rigidbody2D[] bulletRB = new Rigidbody2D[bulletsPerShot];
            IShootable[] shot = new IShootable[bulletsPerShot];
            for (int i = 0; i<bulletsPerShot; i++)
            {
                bullets[i] = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                bulletRB[i] = bullets[i].GetComponent<Rigidbody2D>();
                shot[i] = bullets[i].GetComponent<IShootable>();
            }

            Vector3[] shotDirs;
            shotDirs = GetVectorsInArc();
            for (int i = 0; i < bullets.Length; i++)
            {

                if(bulletRB[i] != null)
                {

                    float dmg = Random.Range(minDamage, maxDamage);
                    bullets[i].transform.up = shotDirs[i];
                    shot[i].SetUpBullet(knockBack, dmg);
                    bulletRB[i].AddForce(shotDirs[i] * shotForce, ForceMode2D.Impulse);
                 
               
                }
                else
                {
                    Destroy(bulletRB[i]);
                }

            }
            currentClip--;
            canShoot = false;
            StartCoroutine(ShotDelay());
        }
    }


    private Vector3[] GetVectorsInArc()
    {
        Vector3[] shotDir = new Vector3[bulletsPerShot];

        for(int i = 0; i<shotDir.Length; i++)
        {
           float startingAngle = (EssoUtility.GetAngleFromVector(firePoint.up) - sprayRange / 2);
           float randOffset = Random.Range(-spray, spray);

            shotDir[i] = EssoUtility.GetVectorFromAngle(randOffset+ startingAngle+sprayRange);
        }

        return shotDir;
    }

}
