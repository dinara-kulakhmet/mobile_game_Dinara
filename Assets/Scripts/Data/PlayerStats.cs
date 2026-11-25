using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Game/Player Stats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;
    public int amountOfJumps;

    [Header("Better jump")]
    public float fallMultiplier;
    public float lowJumpMultiplier;

    [Header("Ground check")]
    public float groundCheckRadius;

    [Header("Health")]
    public float invincibilityTime;
    public float blinkInterval;
    public int startingLives;

    [Header("Attack")]
    public float attackCooldown;
    public Vector2 projectileSpawnOffset;
    public float projectileSpeed;
    public int projectileDamage;
    public float projectileLifetime;
}