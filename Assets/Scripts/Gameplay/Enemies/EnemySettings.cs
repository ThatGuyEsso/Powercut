using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    [Header("Movement Settings")]
    public float maxSpeed; //Max movement speed;
    public float timeZeroToMax; //How fast player accelerate    
    public float timeMaxToZero; //How fast player decellerate
    public float rotationSpeed; // how fast player rotates about it's axis
    public float navAgentSpeedScalar;
    [Header("Stats Settings")]
    public float maxHealth;
    public float maxDamage;
    public float minDamage;
    public float minKnockBack;
    public float maxKnockBack;


    public float knockBackFallOff = 0.1f;
    [Header("Timer Settings")]
    public float hurtTime;
    public float timeBeforeInvulnerable;

    [Header("AI Settings")]
    public float aiTickrate;
    public float nextWaypointDistance;
    public float attackRange;
    public float destroyRange;
    public float destroyRate;
    public float attackRate;
}

