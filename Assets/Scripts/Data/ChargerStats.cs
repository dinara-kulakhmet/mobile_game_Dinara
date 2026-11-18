using UnityEngine;

[CreateAssetMenu(fileName = "ChargerStats", menuName = "Game/Enemy/Charger Stats")]
public class ChargerStats : EnemyStats
{
    [Header("Charge")]
    public float windUp;
    public float chargeSpeed;
    public float maxChargeDistance;
    public float wallCheckDistance;
    public float recoverTime;
    public float hitRadius;
    public int damage;

    [Header("Edge check")]
    public float edgeProbeOffset;
    public float edgeProbeDepth;

    [Header("Hit detection")]
    public float hitPadding;
}