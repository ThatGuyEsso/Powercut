using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBreakVFX 
{
    void Break(Vector2 dir, float force);
    void AddBreakForce(Vector2 dir, float force);
}
