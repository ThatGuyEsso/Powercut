using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShootable 
{
    void SetUpBullet(float knockBack, float damage);
    void Shoot(Vector2 dir, float force);
}
