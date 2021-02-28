using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtFlash : MonoBehaviour
{
    [Header("Materials and Shaders")]
    private Material defaultMaterial;
    public Material hurtMaterial;

    [Header("Effect settings ")]
    public float timeBeforeFlashShift;
   
    private float currentFlashTime;
    private bool isFlashing;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        currentFlashTime = timeBeforeFlashShift;
        defaultMaterial = spriteRenderer.material;
    }

    private void Update()
    {
        if (isFlashing)
        {
            if (currentFlashTime <= 0)
            {
                if(spriteRenderer.material == hurtMaterial)
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
