using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeProjectile : BaseBullet
{
    [SerializeField] protected float rotRate;
    [SerializeField] protected GameObject gfx;
    [SerializeField] protected List<GameObject> slimeFragmentsPrefabs = new List<GameObject>();

    [SerializeField] protected int fragmentCounts;
    private void Update()
    {
        gfx.transform.Rotate(new Vector3(0.0f,0.0f,10.0f)*Time.deltaTime* rotRate);
    }

    override protected void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & collisionLayers) != 0)
        {
            if (!other.CompareTag("Enemy"))
            {
                IAudio audioPlayer = ObjectPoolManager.Spawn(audioPlayerPrefab, transform.position).GetComponent<IAudio>();
                audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound("BulletCollisionSFX"));
                audioPlayer.PlayAtRandomPitch();

                Vector2 backDir = rb.velocity.normalized * -1;
                Vector2 pos = rb.position + backDir * 0.6f;
                ObjectPoolManager.Spawn(sparkPrefab, pos, transform.rotation);
                //SetupAndPlayBulletSound("BulletCollisionSFX");
                SpawnFragments();
                ObjectPoolManager.Recycle(gameObject);
            }
          
        }
        if (other.gameObject.CompareTag(targetTag) || other.gameObject.CompareTag("PhysicsObject"))
        {
            if (other.GetComponent<IHurtable>() != null)
            {
                other.GetComponent<IHurtable>().Damage(damage, rb.velocity.normalized, knockBack);

            }
            ObjectPoolManager.Spawn(sparkPrefab, transform.position, transform.rotation);
            ObjectPoolManager.Spawn(triggerEnemyPrefab, transform.position, Quaternion.identity);

            SpawnFragments();
            ObjectPoolManager.Recycle(gameObject);
        }

     
    }

    override public IEnumerator RecycleAfterTime()
    {
        yield return new WaitForSeconds(3.0f);
        SpawnFragments();
        ObjectPoolManager.Recycle(gameObject);
    }
    public void SpawnFragments()
    {
        float angleIncrement =   360f/ fragmentCounts;
        float currentAngle = 0f;
        GameObject currentFragment;
        for (int i = 0; i < fragmentCounts; i++)
        {
            int rand = Random.Range(0, slimeFragmentsPrefabs.Count);
            currentFragment = ObjectPoolManager.Spawn(slimeFragmentsPrefabs[rand], transform.position);

            Vector3 dir =EssoUtility.GetVectorFromAngle(currentAngle).normalized;
            currentFragment.transform.up = dir;
            IShootable frag = currentFragment.GetComponent<IShootable>();
            frag.SetUpBullet(knockBack / fragmentCounts, damage / fragmentCounts);
            frag.Shoot(dir, shotForce*0.8f);
            currentAngle += angleIncrement;
        }
    }
}
