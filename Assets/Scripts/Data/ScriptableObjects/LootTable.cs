using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "New Loot Table")]
public class LootTable : ScriptableObject
{
    public List<GameObject> loot;

    public GameObject ReturnLoot()
    {
        int randIndex = Random.Range(0, loot.Count);

        return loot[randIndex];
    }
}
