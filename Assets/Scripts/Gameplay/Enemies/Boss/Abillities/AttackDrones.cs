using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AttackDrones : BaseAttackPattern
{

    public SeekingDrones dronePrefab;

    private List<SeekingDrones> activeDrones = new List<SeekingDrones>();
    [SerializeField] private float droneSize = 1f;

    public Action Disabled;
    public override void ExecuteAttack()
    {
        if (playerTransform)
        {
            StartCoroutine(StaggeredDroneAttack());
        }
       
    }
    public void OnDisable()
    {
        StopAllCoroutines();
    }
    public override void BeginAttackPattern()
    {
        base.BeginAttackPattern();
    }
    private IEnumerator StaggeredDroneAttack()
    {

        float range = Vector2.Distance(playerTransform.position, transform.position); 
        //if not in range wait till player is in range
        while(range> attackRange)
        {
            yield return null;
        }
        for (int i = 0; i < attackCount; i++)
        {
         
            if (activeDrones.Count < maxAttackCount)
            {
             
                SendOutDrone();
                yield return new WaitForSeconds(1.0f);
            }
        }
    }
    private void SendOutDrone()
    {
        SeekingDrones drone = ObjectPoolManager.Spawn(dronePrefab, transform.position, Quaternion.identity);
        drone.transform.localScale = new Vector3(droneSize, droneSize, droneSize);
        drone.SetUpDrone(playerTransform, damage, knockBack, this);
        activeDrones.Add(drone);
    }
    public void Dronekilled(SeekingDrones drone)
    {
        activeDrones.Remove(drone);
    }
    public override void DisableAttack()
    {
        base.DisableAttack();
        Disabled?.Invoke();
        activeDrones.Clear();

    }
    public override void StopRunning()
    {
        base.StopRunning();
        StopAllCoroutines();
        Disabled?.Invoke();
        activeDrones.Clear();
    }


   
}
