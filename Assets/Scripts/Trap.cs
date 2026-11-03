using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private Transform playerSpawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() == null) return;

        if (playerSpawnPoint != null)
        {
            collision.transform.position = playerSpawnPoint.position;
        }
        else
        {
            Debug.LogWarning("Trap: Player Spawn Point is not assigned", this);
        }

        if (collision.attachedRigidbody != null)
        {
            collision.attachedRigidbody.linearVelocity = Vector2.zero;
        }

        PlayerHealth health = collision.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage();
        }
    }
}
