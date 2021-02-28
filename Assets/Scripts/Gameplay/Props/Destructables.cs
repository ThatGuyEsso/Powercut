using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructables : MonoBehaviour,IBreakVFX,IHurtable
{
    private int nBrokenParts;
    [SerializeField] private List<GameObject> brokenPartTypes = new List<GameObject>();
    [SerializeField] private List<int> brokenPartsWeight = new List<int>();
    [SerializeField] private List<IBreakVFX> attachedPartedParts= new List<IBreakVFX>();
    [SerializeField] private float spread;
    [SerializeField] private float spreadAngle;

    [SerializeField] private float health= 20.0f;

    private float knockBack;
    private Vector3 direction;
    private void Awake()
    {
        IBreakVFX[] breakables = gameObject.GetComponentsInChildren<IBreakVFX>();
        GetSumOfBrokenParts();
        GetAllBreakableChildren(breakables);

    }
    private void GetSumOfBrokenParts()
    {
        int sum=0;
        for(int i = 0; i < brokenPartsWeight.Count; i++)
        {
            sum += brokenPartsWeight[i];
        }
        nBrokenParts = sum;
    }
    public void Break(Vector2 dir, float force)
    {
        Vector3[] partDir = GetVectorsInArc(dir);
        for (int i= 0; i< brokenPartTypes.Count; i++)
        {
            for(int k = 0; k < brokenPartsWeight[i]; k++)
            {
                GameObject part =ObjectPoolManager.Spawn(brokenPartTypes[i],transform.position);
                part.GetComponent<IBreakVFX>().AddBreakForce(partDir[i], force);
            }
        }

        if (attachedPartedParts.Count > 0)
        {
            foreach(IBreakVFX part in attachedPartedParts)
            {
                part.Break(dir, force);
            }
        }
        Destroy(gameObject);
    }

    public void Damage(float damage, Vector3 knockBackDir, float knockBack)
    {
        health -= damage;
        if(health <= 0)
        {
            Break(knockBackDir, knockBack);
        }
    }

    private void GetAllBreakableChildren(IBreakVFX[] breakables)
    {
        if (breakables.Length > 0)
        {

            for(int i=0; i < breakables.Length; i++)
            {
                attachedPartedParts.Add(breakables[i]);
            }
        }
    }


    private Vector3[] GetVectorsInArc(Vector3 dir)
    {
        Vector3[] partDir = new Vector3[nBrokenParts];

        for (int i = 0; i < partDir.Length; i++)
        {
            float startingAngle = (EssoUtility.GetAngleFromVector(dir) - spreadAngle / 2);
            float randOffset = Random.Range(-spread, spread);

            partDir[i] = EssoUtility.GetVectorFromAngle(randOffset + startingAngle + spreadAngle);
        }

        return partDir;
    }

    public void AddBreakForce(Vector2 dir, float force)
    {
        throw new System.NotImplementedException();
    }
}
