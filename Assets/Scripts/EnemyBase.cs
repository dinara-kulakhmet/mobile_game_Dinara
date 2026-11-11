using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Vision")]
    [SerializeField] protected float visionRange = 6f;
    [SerializeField, Range(10f, 360f)] protected float visionAngle = 90f; 
    [Tooltip("ТОЛЬКО стены/земля. Игрока в этот слой НЕ добавлять.")]
    [SerializeField] protected LayerMask obstacleMask;

    [Header("Telegraph")]
    [SerializeField] protected Color telegraphColor = Color.red;

    [Header("Fairness")]
    [SerializeField] protected bool attackOnlyOnScreen = true;

    protected Transform player;
    protected SpriteRenderer sprite;
    protected Color baseColor = Color.white;
    protected int facing = 1; 

    protected virtual void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        if (sprite != null) baseColor = sprite.color;
    }

    protected virtual void Start()
    {
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    protected float DistanceToPlayer()
    {
        return player != null ? Vector2.Distance(transform.position, player.position) : float.MaxValue;
    }

    protected bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector2 toPlayer = player.position - transform.position;
        float dist = toPlayer.magnitude;
        if (dist > visionRange) return false;

        Vector2 forward = new Vector2(facing, 0f);
        if (Vector2.Angle(forward, toPlayer) > visionAngle * 0.5f) return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, toPlayer.normalized, dist, obstacleMask);
        return hit.collider == null; 
    }

    protected bool CanAttackNow()
    {
        if (!attackOnlyOnScreen) return true;
        Camera cam = Camera.main;
        if (cam == null) return true;
        Vector3 v = cam.WorldToViewportPoint(transform.position);
        return v.z > 0f && v.x > 0f && v.x < 1f && v.y > 0f && v.y < 1f;
    }

    protected void SetFacing(int dir)
    {
        if (dir == 0) return;
        facing = dir > 0 ? 1 : -1;
        if (sprite != null) sprite.flipX = facing < 0;
    }

    protected void SetTelegraph(bool on)
    {
        if (sprite != null) sprite.color = on ? telegraphColor : baseColor;
    }

    protected void DealDamage(GameObject target, int damage)
    {
        if (target == null) return;
        PlayerHealth health = target.GetComponentInParent<PlayerHealth>();
        if (health != null) health.TakeDamage();
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }
}