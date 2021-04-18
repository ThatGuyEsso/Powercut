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


    public void InitLampView()
    {
        ///lamps shadows do not change very often.
        /// so to impove performance reduce update rate     
        InvokeRepeating("UpdateConeView", 0.0f, lampSettings.lightTickRate);
    }

    protected override void LateUpdate()
    {
        
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
