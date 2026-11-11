using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 6f;
    [SerializeField] private int damage = 1;
    [SerializeField] private float lifetime = 4f;
    [SerializeField] private LayerMask obstacleMask;

    private Vector2 direction;

    public void Launch(Vector2 dir)
    {
        direction = dir.normalized;
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