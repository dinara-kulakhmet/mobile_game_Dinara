using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleMask;

    private Vector2 direction;
    private float speed;

    public void Launch(Vector2 dir, float projectileSpeed, float lifetime)
    {
        direction = dir.normalized;
        speed = projectileSpeed;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        transform.position += (Vector3)(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponentInParent<PlayerHealth>();
            if (health != null) health.TakeDamage();
            Destroy(gameObject);
        }
        else if (((1 << other.gameObject.layer) & obstacleMask) != 0)
        {
            Destroy(gameObject);
        }
    }
}