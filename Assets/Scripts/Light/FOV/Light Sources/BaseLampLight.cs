using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseLampLight : FieldOfView
{
    public  LampSettings lampSettings;

    protected override void Awake()
    {
        base.Awake();
    }

    override protected void Update()
    {
        base.Update();
       
    }

    override protected void SetUpLight()
    {
        origin = Vector3.zero;
        fovAngle = lampSettings.lightAngle;
        ViewBlockingLayers = lampSettings.lightBlockingLayers;
        viewDistance = lampSettings.lightRadius;
        lightIsOn = true;
        offset = lampSettings.lightAngle;
        rayCount = lampSettings.rayCount;
        enemyLayer = settings.enemyLayer;
    }


    public void AddLightDistanceModifier(float modifier)
    {
        viewDistance += modifier;
    }
}
