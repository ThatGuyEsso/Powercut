using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BaseAttackPattern : MonoBehaviour
{
    private BossStage stage;
    public void SetStage(BossStage newStage) { stage = newStage; }
}
