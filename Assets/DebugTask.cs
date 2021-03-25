using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTask : MonoBehaviour,IBreakable
{

    [SerializeField] private float health;
    public void Damage(float damage, BaseEnemy interfacingEnemy)
    {
        health -= damage;
        if (health <= 0)
        {
            interfacingEnemy.GetComponent<IBreakable>().ObjectIsBroken();
            Destroy(gameObject);
        }
    }

    public void ObjectIsBroken()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
