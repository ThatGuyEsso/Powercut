using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtflash : MonoBehaviour
{
    [Header("Materials and Shaders")]
    public Material defaultMaterial;
    public Material hurtMaterial;
    private Material currMat;
    [Header("Effect settings ")]
    public float timeBeforeFlashShift;

    private float currentFlashTime;
    private bool isFlashing;
    public SpriteRenderer spriteRenderer;


    private void Update()
    {
        if (isFlashing)
        {
            if (currentFlashTime <= 0)
            {
                if (currMat == hurtMaterial)
                {
                    spriteRenderer.material = defaultMaterial;
                }
                else
                {
                    spriteRenderer.material = hurtMaterial;
                }
                currentFlashTime = timeBeforeFlashShift;
            }
            else
            {
                currentFlashTime -= Time.deltaTime;
            }
        }
    }


    public void BeginFlash()
    {
        isFlashing = true;
        spriteRenderer.material = hurtMaterial;
    }
    public void EndFlash()
    {
        isFlashing = false;
        spriteRenderer.material = defaultMaterial;
    }
}
