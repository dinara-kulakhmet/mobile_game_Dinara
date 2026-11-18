using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BossProjectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private float ignoreWallsUntil;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float speed, float lifetime, float ignoreWallsFor = 0f)
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

        if (other.isTrigger) return;

        if (other.GetComponentInParent<FinalBoss>() != null) return;

        if (Time.time < ignoreWallsUntil) return;

        Destroy(gameObject);
    }
}