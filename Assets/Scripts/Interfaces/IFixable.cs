using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFixable 
{
    bool CanFix();
    void NotFixing();
}
