using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteFlash : MonoBehaviour
{
    [Header("Materials and Shaders")]
    private Sprite defaultSprite;
    [SerializeField] private Sprite flashSprite;

    [Header("Effect settings ")]
    public float timeBeforeFlashShift;

    private float currentFlashTime;
    private bool isFlashing;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        currentFlashTime = timeBeforeFlashShift;
        defaultSprite = spriteRenderer.sprite;
    }

    private void Update()
    {
        if (isFlashing)
        {
            if (currentFlashTime <= 0)
            {
                if (spriteRenderer.sprite == flashSprite)
                {
                    spriteRenderer.sprite = defaultSprite;
                }
                else
                {
                    spriteRenderer.sprite = flashSprite;
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
        spriteRenderer.sprite = flashSprite;
    }
    public void EndFlash()
    {
        isFlashing = false;
        spriteRenderer.sprite = defaultSprite;
    }
}
