using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnbreakableLamp : Lamp
{
    [SerializeField] private BaseLampLight lightPrefab;
    public override void Init()
    {
        base.Init();
        BaseLampLight light= Instantiate(lightPrefab.gameObject, Vector3.zero,Quaternion.identity).GetComponent<BaseLampLight>();

        lightRef = light;

        lightRef.SetOrigin(transform.position);
        lightRef.SetAimDirection(transform.up);
        lightRef.InitLampView();
        lightRef.ToggleLight(isLampWorking);
    }
    // Update is called once per frame
    override protected void Update()
    {
        
    }
}
