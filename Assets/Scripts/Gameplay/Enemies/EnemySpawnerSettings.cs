using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Enemy Spawner Settings")]
public class EnemySpawnerSettings : ScriptableObject
{
    public List<GameObject> enemyTypes = new List<GameObject>();
    public float maxTimeBeforeSpawn;
    public float minTimeBeforeSpawn;
    public int maxNumberSpawned;
    public int minNumberSpawned;
    public float spawnRadius;
}
