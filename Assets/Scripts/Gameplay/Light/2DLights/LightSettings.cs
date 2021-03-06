
using UnityEngine;

[CreateAssetMenu(menuName ="Light Settings")]
public class LightSettings : ScriptableObject
{
    public float lightRadius;
    public int rayCount;
    [Range(0, 390)]
    public float lightAngle = 90f;
    public LayerMask lightBlockingLayers;
    public LayerMask enemyLayer;
    public float maxCharge;
    public float dischargeRate;
    public float chargeRate;
    public float rechargeAmount;
    public float disChargeAmount;
    public float maxIntensity;


}
