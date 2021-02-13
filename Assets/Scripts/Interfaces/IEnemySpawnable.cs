using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemySpawnable
{
    void LampInDarkness();

    void LampInLight();

    void Spawn();

    void SetUp();
}
