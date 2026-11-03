using UnityEngine;

// Prefab setup: SpriteRenderer + Rigidbody2D (Dynamic) + Collider2D with Is Trigger on + this script.
[RequireComponent(typeof(Rigidbody2D))]
public class BossProjectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 6f;

    private Rigidbody2D rb;
    private float ignoreWallsUntil;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // ignoreWallsFor: the rain is spawned above the player, maybe inside the ceiling,
    // so for a short time it flies through walls
    public void Launch(Vector2 direction, float speed, float ignoreWallsFor = 0f)
    {
        rb.gravityScale = 0f;
        rb.linearVelocity = direction.normalized * speed;
        ignoreWallsUntil = Time.time + ignoreWallsFor;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerHealth health = other.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage();
            Destroy(gameObject);
            return;
        }

        // Pickups, traps, other projectiles
        if (other.isTrigger) return;

        // The boss itself (the projectile is spawned inside it)
        if (other.GetComponentInParent<FinalBoss>() != null) return;

        if (Time.time < ignoreWallsUntil) return;

        Destroy(gameObject);
    }
}
