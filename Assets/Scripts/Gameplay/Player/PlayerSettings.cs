using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Player Settings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Movement Settings")]
    public float maxSpeed; //Max movement speed;
    public float timeZeroToMax; //How fast player accelerate    
    public float timeMaxToZero; //How fast player decellerate
    public float rotationSpeed; // how fast player rotates about it's axis
    public float walkSensitivity;

    [Header("health Settings")]
    public float maxHealth;
    public float maxHurtTime;

 
    [Header("ScreenShake Settings")]
    public float duration, smoothIn, smoothOut,magnitude;

}
