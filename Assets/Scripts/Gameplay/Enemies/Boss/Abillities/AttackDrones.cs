using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDrones : BaseAttackPattern
{

    public SeekingDrones dronePrefab;
    private List<SeekingDrones> activeDrones = new List<SeekingDrones>();
    public override void ExecuteAttack()
    {
        StartCoroutine(StaggeredDroneAttack());
    }

    private IEnumerator StaggeredDroneAttack()
    {
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
        drone.SetUpDrone(playerTransform, damage, knockBack, this);
        activeDrones.Add(drone);
    }
    public void Dronekilled(SeekingDrones drone)
    {
        activeDrones.Remove(drone);
    }

}
