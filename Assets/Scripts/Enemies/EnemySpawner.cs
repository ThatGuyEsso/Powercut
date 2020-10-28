using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour,IEnemySpawnable
{
    public EnemySpawnerSettings settings;
    private float currTimeBeforeSpawn;
    private int spawnAmount;
    private bool canSpawn;
    private void Awake()
    {
        ResetSpawnTimer();
        
    }
    private void FixedUpdate()
    {
        if (canSpawn)
        {
            if (currTimeBeforeSpawn <=0){

                SpawnEnemies();
                ResetSpawnTimer();
            }
            else
            {
                currTimeBeforeSpawn -= Time.deltaTime;
            }
        }
        
    }
    //Spawns enemies and sets their target
    private void SpawnEnemies()
    {
        if (canSpawn)
        {
            canSpawn = false;
            Transform target = LevelLampsManager.instance.GetNearestFuseLightFuse(transform);
            if (target == null)
            {
                target = FindObjectOfType<PlayerBehaviour>().transform;
            }

            spawnAmount = Random.Range(settings.minNumberSpawned, settings.maxNumberSpawned);
            for(int i =0; i < spawnAmount; i++)
            {
                int rand = Random.Range(0, settings.enemyTypes.Count - 1);

                BaseEnemy currEnemy = Instantiate(settings.enemyTypes[rand],(Random.insideUnitCircle*settings.spawnRadius+(Vector2)transform.position),Quaternion.identity).GetComponent<BaseEnemy>();
                currEnemy.SetTarget(target);
            }
        }
    }

    private void ResetSpawnTimer()
    {
        currTimeBeforeSpawn = Random.Range(settings.minTimeBeforeSpawn, settings.maxTimeBeforeSpawn);
    }



    //Spawnable Interface
    void IEnemySpawnable.LampInDarkness()
    {
        canSpawn = true;
    }

    void IEnemySpawnable.LampInLight()
    {
      
        canSpawn = false;
    }
}
