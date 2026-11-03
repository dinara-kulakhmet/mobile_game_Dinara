using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private int pointsToAdd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() == null) return;

        if (PointsManager.Instance != null)
        {
            PointsManager.Instance.IncreasePoints(pointsToAdd);
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayCollectibleSound();
        }

        Destroy(gameObject);
    }
}