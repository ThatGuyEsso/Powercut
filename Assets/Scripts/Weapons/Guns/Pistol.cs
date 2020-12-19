using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : BaseGun
{
    override protected void Awake()
    {
        base.Awake();
        //set appropriate gun type
        gunType = GunTypes.Pistol;
    }
}
