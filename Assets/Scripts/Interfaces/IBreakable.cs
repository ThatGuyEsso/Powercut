using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBreakable 
{
    void Damage(float damage,BaseEnemy interfacingEnemy);

    void ObjectIsBroken();
}
