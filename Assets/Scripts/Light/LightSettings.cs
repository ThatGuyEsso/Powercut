
using UnityEngine;

[CreateAssetMenu(menuName ="Light Settings")]
public class LightSettings : ScriptableObject
{
    public float lightRadius;
    [Range(0, 360)]
    public float lightAngle = 90f;
    public LayerMask lightBlockingLayers;
    public float maxCharge;
    public float dischargeRate;


}
