using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSpriteHurtFlash : MonoBehaviour
{
    [Header("Materials and Shaders")]
    public Material defaultMaterial;
    public Material hurtMaterial;
    private Material currMat;
    [Header("Effect settings ")]
    public float timeBeforeFlashShift;

    private float currentFlashTime;
    private bool isFlashing;
    public List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    private void Awake()
    {
        Init();
    }
    private void Update()
    {
        if (isFlashing)
        {
            if (currentFlashTime <= 0)
            {
                if (currMat == hurtMaterial)
                {
                    SetSpriteMaterials(defaultMaterial);
                }
                else
                {
                    SetSpriteMaterials(hurtMaterial);
                }
                currentFlashTime = timeBeforeFlashShift;
            }
            else
            {
                currentFlashTime -= Time.deltaTime;
            }
        }
    }
    public void SetSpriteMaterials(Material material)
    {

        foreach(SpriteRenderer renderer in spriteRenderers)
        {

            renderer.material = material;
            currMat = material;
        }
    }

    public void BeginFlash()
    {
        isFlashing = true;
        SetSpriteMaterials(hurtMaterial);
    }
    public void EndFlash()
    {
        isFlashing = false;
        SetSpriteMaterials(defaultMaterial);
    }

    public void Init()
    {
        currentFlashTime = timeBeforeFlashShift;
        SpriteRenderer[] renderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach(SpriteRenderer sr in renderers)
        {
            spriteRenderers.Add(sr);
        }
    }

}
