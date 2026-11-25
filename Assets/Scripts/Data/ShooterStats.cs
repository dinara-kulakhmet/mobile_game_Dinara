using UnityEngine;

[CreateAssetMenu(fileName = "ShooterStats", menuName = "Game/Enemy/Shooter Stats")]
public class ShooterStats : EnemyStats
{
    [Header("Shooting")]
    public float aimTime;
    public float cooldown;
    public float projectileSpeed;
    public float projectileLifetime;
}