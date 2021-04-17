using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BroodNestDeathHandler : MonoBehaviour
{
    [SerializeField] private SpriteRenderer bossGFX;
    [SerializeField] private Sprite deathSprite;

    public void InitDeathState()
    {
        bossGFX.sprite = deathSprite;
    }

}
