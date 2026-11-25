using UnityEngine;

[CreateAssetMenu(fileName = "PatrollerStats", menuName = "Game/Enemy/Patroller Stats")]
public class PatrollerStats : EnemyStats
{
    [Header("Patrol")]
    public float patrolSpeed;
    public float waypointWait;

    [Header("Chase")]
    public float chaseSpeed;
    public float loseSightTime;

    [Header("Attack")]
    public float attackRange;
    public float windUp;
    public float attackCooldown;
    public int damage;

    [Header("Contact damage")]
    public float contactCooldown;

    [Header("Edge check")]
    public float edgeProbeOffset;
    public float edgeProbeDepth;
}