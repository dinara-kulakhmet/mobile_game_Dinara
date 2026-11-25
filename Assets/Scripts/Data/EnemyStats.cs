using UnityEngine;

public abstract class EnemyStats : ScriptableObject
{
    [Header("Vision")]
    public float visionRange;
    [Range(10f, 360f)] public float visionAngle;
}