using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHurtable
{
    void Damage(float damage, Vector3 knockBackDir, float knockBack);

    void Push(Vector3 knockBackDir, float knockBack);
}
