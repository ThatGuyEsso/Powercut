using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructables : MonoBehaviour,IBreakVFX,IHurtable
{
    private int nBrokenParts;
    [SerializeField] private List<GameObject> brokenParts = new List<GameObject>();

    [SerializeField] private float spread;
    [SerializeField] private float spreadAngle;

    [SerializeField] private float health= 20.0f;

    private float knockBack;
    private Vector3 direction;
    bool isBroken = false;

    public void Break(Vector2 dir, float force)
    {
        Vector3[] partDir = EssoUtility.GetVectorsInArc(dir, brokenParts.Count, spreadAngle, spread);
        for (int i= 0; i< brokenParts.Count; i++)
        {
         
          
                GameObject part =ObjectPoolManager.Spawn(brokenParts[i],transform.position);
                part.GetComponent<IBreakVFX>().AddBreakForce(partDir[i], force);

            Debug.DrawRay(transform.position, partDir[i], Color.yellow, 10f);
            
        }

        Destroy(gameObject);
    }

    public void Damage(float damage, Vector3 knockBackDir, float knockBack)
    {
        health -= damage;
        if(health <= 0&&!isBroken)
        {
            isBroken = true;
            Break(knockBackDir, knockBack);
        }
    }

   


    public void AddBreakForce(Vector2 dir, float force)
    {
        throw new System.NotImplementedException();
    }
}
