using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private int damage;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, PlayerStats stats)
    {
        damage = stats.projectileDamage;

        rb.gravityScale = 0f;
        rb.linearVelocity = direction.normalized * stats.projectileSpeed;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (direction.x < 0f ? -1f : 1f);
        transform.localScale = scale;

        Destroy(gameObject, stats.projectileLifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null) return;

        FinalBoss boss = other.GetComponentInParent<FinalBoss>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        if (other.isTrigger) return;

        Destroy(gameObject);
    }
}