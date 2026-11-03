using UnityEngine;

// Prefab setup: SpriteRenderer + Rigidbody2D (Dynamic) + Collider2D with Is Trigger on + this script.
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 3f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float speed)
    {
        rb.gravityScale = 0f;
        rb.linearVelocity = direction.normalized * speed;

        // Turn the sprite to the flying direction
        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * (direction.x < 0f ? -1f : 1f);
        transform.localScale = scale;

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Never hit the shooter
        if (other.GetComponent<PlayerController>() != null) return;

        FinalBoss boss = other.GetComponentInParent<FinalBoss>();
        if (boss != null)
        {
            boss.TakeDamage(damage);
            Destroy(gameObject);
            return;
        }

        // Pickups, traps, portals and other triggers do not stop the projectile
        if (other.isTrigger) return;

        // Ground and walls
        Destroy(gameObject);
    }
}
