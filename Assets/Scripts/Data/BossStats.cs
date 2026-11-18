using UnityEngine;

[CreateAssetMenu(fileName = "BossStats", menuName = "Game/Enemy/Boss Stats")]
public class BossStats : ScriptableObject
{
    [Header("Health")]
    public int maxHealth;

    [Header("Activation")]
    public float activationRange;

    [Header("Visuals")]
    public float hoverHeight;
    public float hoverSpeed;
    public float hitFlashTime;
    public float deathFadeTime;

    [Header("Attacks")]
    public float projectileSpeed;
    public float firstAttackDelay;
    public float attackInterval;
    public float phaseTwoAttackInterval;
    public float projectileLifetime;

    [Header("Fan attack")]
    public int fanProjectiles;
    public int phaseTwoFanProjectiles;
    public float fanAngle;

    [Header("Rain attack")]
    public int rainProjectiles;
    public int phaseTwoRainProjectiles;
    public float rainSpread;
    public float rainSpawnHeight;
    public float rainWarningTime;
    public float rainWallIgnoreTime;
}