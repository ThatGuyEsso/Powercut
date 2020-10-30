﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    public static Vector2 GetVectorToPointer(Camera camRef, Vector3 orign)
    {
        //Get mouse position in world space
        Vector3 pointerPos = camRef.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector3 orignToMouse = pointerPos - orign;//calculate vector direction between player and cursor

        return orignToMouse.normalized;//Return normalised direction
    }
}
