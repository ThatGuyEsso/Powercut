
using UnityEngine;
[CreateAssetMenu(menuName = "Lamp Settings")]
public class LampSettings : ScriptableObject
{
    [Header("LightShapeSettings")]
    public float lightRadius;
    public int rayCount;
    [Range(0, 400)]
    public float lightAngle = 90f;
    public LayerMask lightBlockingLayers;

    public float maxLightHealth;
}

