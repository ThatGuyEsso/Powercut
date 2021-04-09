using System.Collections;
using System;
using UnityEngine;

[Serializable]
public class AttackPatternData
{
    [SerializeField] private float attackRate;
    [SerializeField] private float attackDuration;
    [SerializeField] private float attackCoolDown;
    [SerializeField] private int attackCount;
    [SerializeField] private int maxAttackCount;
    [SerializeField] private BaseAttackPattern pattern;
    private BossStage stage;


    public float SetAttackRate { set { attackRate = value; } }
    public float AttackRate { get { return attackRate ; } }
    public float SetAttackDuration { set { attackDuration = value; } }
    public float AttackDuration { get { return attackDuration; } }
    public float SetCooldown { set { attackCoolDown = value; } }
    public float Cooldown { get { return attackCoolDown; } }
    public int SetAttackCount { set { attackCount = value; } }
    public int AttackCount { get { return attackCount; } }

    public int SetMaxAttackCount { set { maxAttackCount = value; } }
    public int MaxAttackCount { get { return maxAttackCount; } }

    public BossStage SetStage { set { stage = value; } }
    public BossStage Stage { get { return stage; } }
    public BaseAttackPattern SetAttackPattern { set { pattern = value; } }
    public BaseAttackPattern AttackPattern { get { return pattern; } }
}
