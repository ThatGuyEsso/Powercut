using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssoUtility : MonoBehaviour
{

    //Converts angle into vector
    public static float GetAngleFromVector(Vector3 vector)
    {
        vector = vector.normalized;
        float n = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360f;
        return n;
    }

    //returns angle from vector direction

    public static Vector3 GetVectorFromAngle(float angle)
    {
        //angle -> 360
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
