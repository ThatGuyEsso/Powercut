using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuzzleFlash : MonoBehaviour
{
    SpriteRenderer muzzleSFX;

    public void Init()
    {
        muzzleSFX = gameObject.GetComponent<SpriteRenderer>();
        muzzleSFX.enabled = false;
    }

    public void BeginMuzzleFash(float flashDur, Transform flashPoint)
    {
        transform.position = flashPoint.position+flashPoint.up*0.2f;
        muzzleSFX.enabled = true;
        Invoke("EndFlash", flashDur);
    }

    private void EndFlash()
    {
        muzzleSFX.enabled = false;
    }
}
