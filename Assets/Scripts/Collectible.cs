using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private ItemData item;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() == null) return;

        if (item == null)
        {
            Debug.LogWarning("Collectible: ItemData is not assigned", this);
            return;
        }

        if (PointsManager.Instance != null)
        {
            PointsManager.Instance.IncreasePoints(item.points);
        }
        GameSession.Score += item.points;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayCollectibleSound();
        }

        Destroy(gameObject);
    }
}