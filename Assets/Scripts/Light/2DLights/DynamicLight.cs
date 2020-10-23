using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class DynamicLight : MonoBehaviour
{
    [Range(0, 360)]
    private float fovAngle = 90f;

    [HideInInspector]
    public float currentAngle;
    public float viewDistance;

    private LayerMask ViewBlockingLayers;
    private Light2D lightCone;
    Vector3 origin;
    float startingAngle;
    Vector3[] lightPathPoints;
    public float offset;

    private bool lightIsOn;
    public LightSettings settings;
    public LightManager manager;
    public void Awake()
    {

        lightCone = gameObject.GetComponent<Light2D>();
        origin = Vector3.zero;
        lightPathPoints= new Vector3[lightCone.shapePath.Length];//get number of points on shape
        manager = gameObject.GetComponent<LightManager>();
        SetUpLight();
    }
    // Start is called before the first frame update



    public void Update()
    {


        //origin = Vector3.zero;// transform.parent.position;
        if (lightIsOn)
        {
            SetShapeOfLight();
        }


    }

    //Updates the origin of light source
    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    //Sets the starting angle to direction of given vector
    public void SetAimDirection(Vector3 aimDir)
    {
        startingAngle = (EssoUtility.GetAngleFromVector(aimDir) - fovAngle / 2) ;
    }



  

    //Update light shape
    private void SetShapeOfLight()
    {
        Debug.Log("running in light");
        currentAngle = startingAngle + offset;
        float angleIncrease = fovAngle / lightPathPoints.Length;
        for (int i = 0; i < lightPathPoints.Length; i++)
        {



            if (i > 0)
            {
                RaycastHit2D hitInfo = Physics2D.Raycast(origin, EssoUtility.GetVectorFromAngle(currentAngle), viewDistance, ViewBlockingLayers);
                if (hitInfo)
                {
                    lightPathPoints[i] = hitInfo.point;
                    Debug.DrawLine(lightPathPoints[0], hitInfo.point);
                }
                else
                {
                    lightPathPoints[i] = origin + EssoUtility.GetVectorFromAngle(currentAngle) * viewDistance;
                    Debug.DrawRay(lightPathPoints[0], lightPathPoints[0] + EssoUtility.GetVectorFromAngle(currentAngle) * viewDistance);
                }


            }
            else
            {
                lightPathPoints[i] = origin;
            }


            currentAngle -= angleIncrease;
        }
        for (int i = 0; i < lightCone.shapePath.Length; i++)
        {
            lightCone.shapePath[i] = lightPathPoints[i];
        }
    }

    private void SetUpLight()
    {
        fovAngle = settings.lightAngle;
        ViewBlockingLayers = settings.lightBlockingLayers;
        viewDistance = settings.lightRadius;
        lightIsOn = true;
        offset = settings.lightAngle;
    }


    public void ToggleLight(bool isOn)
    {
        lightIsOn = isOn;
        if (lightIsOn)
        {
            lightCone.intensity = settings.maxIntensity;
        }
        else
        {
            lightCone.intensity = 0f;
        }
    }


    public bool GetLightIsOn()
    {
        return lightIsOn;
    }
}
