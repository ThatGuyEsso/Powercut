using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEventHandler : MonoBehaviour
{
    [SerializeField]
    private string stepSFX;


    [SerializeField] private GameObject dustParticleEffects;
    public void OnStrideFinished()
    {
        AudioManager.instance.PlayRandFromGroup(stepSFX);
        KickUpDust();
    }

    public void KickUpDust()
    {
        if (dustParticleEffects)
        {

            GameObject dustVFX = ObjectPoolManager.Spawn(dustParticleEffects, transform.position, Quaternion.identity);
            dustVFX.transform.up = transform.up * -1f;
        }
    }
}
