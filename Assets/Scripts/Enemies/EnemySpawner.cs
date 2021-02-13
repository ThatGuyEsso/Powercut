using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour,IEnemySpawnable
{
    public EnemySpawnerSettings settings;
    private float currTimeBeforeSpawn;
    private int spawnAmount;
    private bool canSpawn;
    private bool isAtMax =false;


    public void Init()
    {
        ResetSpawnTimer();
        GameIntensityManager.instance.OnLimitReached += OnEnemyCap;
        GameIntensityManager.instance.OnLimitReached += OnEnemyCapRemoved;
    }
    private void Update()
    {
        if (!isAtMax)
        {
            if (canSpawn && !GameIntensityManager.instance.GetIsAtCrawlerLimit())
            {
                if (currTimeBeforeSpawn <= 0)
                {

                    SpawnEnemies();
                    ResetSpawnTimer();
                }
                else
                {
                    currTimeBeforeSpawn -= Time.deltaTime;
                }
            }
        }

        
    }
    //Spawns enemies and sets their target
    public void SpawnEnemies()
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
                if (!GameIntensityManager.instance.GetIsAtCrawlerLimit())
                {

                    int rand;
                    if (settings.enemyTypes.Count > 1)
                    {
                     rand = Random.Range(0, settings.enemyTypes.Count - 1);

                    }
                    else
                    {
                        rand = 0;
                    }

                    BaseEnemy currEnemy = ObjectPoolManager.Spawn(settings.enemyTypes[rand],
                        (Random.insideUnitCircle*settings.spawnRadius+(Vector2)transform.position),Quaternion.identity).GetComponent<BaseEnemy>();
                    currEnemy.SetTarget(target);
                    GameIntensityManager.instance.IncrementNumberOfCrawlers();

                }
                else
                {
                    break;
                }
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
        SpawnEnemies();
    }

    void IEnemySpawnable.LampInLight()
    {
      
        canSpawn = false;
    }

    void IEnemySpawnable.Spawn()
    {
        canSpawn = true;
        SpawnEnemies();
        ResetSpawnTimer();
    }


    private void OnEnemyCap()
    {
        isAtMax = true;
    }

    private void OnEnemyCapRemoved()
    {
        isAtMax = false;
    }

    private void OnDestroy()
    {
        GameIntensityManager.instance.OnLimitReached -= OnEnemyCap;
        GameIntensityManager.instance.OnLimitReached -= OnEnemyCapRemoved;
    }

    public void SetUp()
    {
        Init();
    }
}
