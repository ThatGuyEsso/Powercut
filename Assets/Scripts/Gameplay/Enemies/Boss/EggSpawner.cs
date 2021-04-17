using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSpawner : MonoBehaviour
{
    private Transform target;
    private GameObject enemyToSpawn;
    [SerializeField] private GameObject hatchVFX;
    [SerializeField] private GameObject audioPlayerPrefab;
    [SerializeField] private string hatchSFX;
    private Rigidbody2D rb;
    private SendSoldiers sourceRef;

    [SerializeField] private float minHatchTime, maxHatchTime;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public void SetUpEgg(Transform target, GameObject enemy, SendSoldiers source)
    {
        this.target = target;
        enemyToSpawn = enemy;
        sourceRef = source;
        float rand = Random.Range(minHatchTime, maxHatchTime);
        Invoke("Hatch", rand);
    }
    public void EjectEgg(float force, Vector2 dir)
    {
        rb.AddForce(force * dir, ForceMode2D.Impulse);
    }
    public void Hatch()
    {
        IAudio audioPlayer = ObjectPoolManager.Spawn(audioPlayerPrefab, transform.position).GetComponent<IAudio>();
        audioPlayer.SetUpAudioSource(AudioManager.instance.GetSound(hatchSFX));
        audioPlayer.PlayAtRandomPitch();
        ObjectPoolManager.Spawn(hatchVFX, transform.position, Quaternion.identity);

        BaseEnemy enemy = ObjectPoolManager.Spawn(enemyToSpawn, transform.position, Quaternion.identity).GetComponent<BaseEnemy>();
        enemy.SetTarget(target);
        sourceRef.BindToSpawnedEnemy(enemy);
        ObjectPoolManager.Recycle(this);

    }
    
}
