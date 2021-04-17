using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendSoldiers : BaseAttackPattern
{

    [SerializeField] private List<GameObject> stageOneEnemies = new List<GameObject>();
    [SerializeField] private List<GameObject> stageTwoEnemies = new List<GameObject>();
    [SerializeField] private List<GameObject> stageThreeEnemies = new List<GameObject>();
    [SerializeField] private List<GameObject> stageFourEnemies = new List<GameObject>();

    private List<GameObject> activeEnemyList = new List<GameObject>();

    [SerializeField] private EggSpawner eggSpawnerPrefab;


    [SerializeField] private float ejectForce;
    private int enemyCount = 0;

    public void Awake()
    {
        aSource = GetComponent<AudioSource>();

        if (aSource)
        {
            Sound droneSFX = AudioManager.instance.GetSound(sfxName);

            aSource.clip = droneSFX.clip;
            aSource.volume = droneSFX.volume;
            aSource.outputAudioMixerGroup = droneSFX.mixerGroup;
            aSource.pitch = droneSFX.pitch;
            aSource.loop = droneSFX.loop;
        }
    }
    public override void ExecuteAttack()
    {
        StartCoroutine(StaggeredEggEjection());
    }
    public override void BeginAttackPattern()
    {
        base.BeginAttackPattern();
        EvaluateActiveEnemies();
    }
    private void EvaluateActiveEnemies()
    {
        switch (stage)
        {
            case BossStage.First:
                activeEnemyList = stageOneEnemies;
                break;
            case BossStage.Second:
                activeEnemyList = stageTwoEnemies;
                break;
            case BossStage.Third:
                activeEnemyList = stageThreeEnemies;
                break;
            case BossStage.Final:
                activeEnemyList = stageFourEnemies;
                break;
        }
    }
    private IEnumerator StaggeredEggEjection()
    {
        for (int i = 0; i < attackCount; i++)
        {

            //if (activeDrones.Count < maxAttackCount)
            //{

                EjectSpawnEgg();
                yield return new WaitForSeconds(1.0f);
            //}
        }
    }

    public GameObject GetRandomEnemyFromList()
    {
        int random = Random.Range(0, activeEnemyList.Count - 1);
        return activeEnemyList[random];
    }
    public void EjectSpawnEgg()
    {
        if(enemyCount < maxAttackCount)
        {
            if (gameObject.activeInHierarchy && aSource) aSource.Play();
            EggSpawner egg = ObjectPoolManager.Spawn(eggSpawnerPrefab, transform.position, Quaternion.identity);
            Vector2 randDirection = EssoUtility.GetVectorFromAngle(Random.Range(0f, 360f));
            egg.EjectEgg(ejectForce, randDirection);
            egg.SetUpEgg(playerTransform, GetRandomEnemyFromList(),this);
        }

        

    }
    
    private void DecrementEnemyCount(BaseEnemy enemy)
    {
        enemyCount--;
        enemy.Killed -= DecrementEnemyCount;
        if (enemyCount < 0) enemyCount = 0;
    }
    public void BindToSpawnedEnemy(BaseEnemy enemy)
    {
        if (enemy)
        {
            enemy.Killed += DecrementEnemyCount;
            enemyCount++;
        }
     

    }
}
