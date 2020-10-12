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
    public Transform playerTrans;
    private LayerMask ViewBlockingLayers;
    private Light2D lightCone;
    Vector3 origin;
    float startingAngle;
    Vector3[] lightPathPoints;
    public float offset;
    private float currentCharge;
    public LightSettings settings;
    public void Awake()
    {

        lightCone = gameObject.GetComponent<Light2D>();
        origin = Vector3.zero;
        lightPathPoints= new Vector3[lightCone.shapePath.Length];//get number of points on shape
        SetUpLight();
    }
    // Start is called before the first frame update



    public void Update()
    {

      
        //origin = Vector3.zero;// transform.parent.position;
        SetShapeOfLight();


    }
    //Converts angle into vector
    public Vector3 GetVectorFromAngle(float angle)
    {
        //angle -> 360
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    //Updates the origin of light source
    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    //Sets the starting angle to direction of given vector
    public void SetAimDirection(Vector3 aimDir)
    {
        startingAngle = (GetAngleFromVector(aimDir) - fovAngle / 2) ;
    }

    //returns angle from vector direction
    public float GetAngleFromVector(Vector3 vector)
    {
        vector = vector.normalized;
        float n = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360f;
        return n;
    }

    //Update position to follow player
    private void FollowPlayer()
    {
        transform.position = playerTrans.position;
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
                RaycastHit2D hitInfo = Physics2D.Raycast(origin, GetVectorFromAngle(currentAngle), viewDistance, ViewBlockingLayers);
                if (hitInfo)
                {
                    lightPathPoints[i] = hitInfo.point;
                    Debug.DrawLine(lightPathPoints[0], hitInfo.point);
                }
                else
                {
                    lightPathPoints[i] = origin + GetVectorFromAngle(currentAngle) * viewDistance;
                    Debug.DrawRay(lightPathPoints[0], lightPathPoints[0] + GetVectorFromAngle(currentAngle) * viewDistance);
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
        currentCharge = settings.maxCharge;
        offset = settings.lightAngle;
    }
}
