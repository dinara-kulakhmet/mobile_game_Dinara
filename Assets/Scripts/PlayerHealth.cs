using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private PlayerStats stats;
    private SpriteRenderer spriteRenderer;
    private bool isInvincible;

    public bool IsInvincible => isInvincible;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null) stats = controller.Stats;
    }

    private void OnDisable()
    {
        SetAlpha(1f);
        isInvincible = false;
    }

    public bool TakeDamage()
    {
        if (isInvincible) return false;

        GameOverManager gameOverManager = FindAnyObjectByType<GameOverManager>();
        if (gameOverManager != null)
        {
            gameOverManager.ReduceLives();
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayHurtSound();
        }

        if (Time.timeScale > 0f && stats != null)
        {
            StartCoroutine(InvincibilityRoutine());
        }

        return true;
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        float timer = 0f;
        while (timer < stats.invincibilityTime)
        {
            if (spriteRenderer != null)
            {
                SetAlpha(spriteRenderer.color.a > 0.9f ? 0.3f : 1f);
            }

            yield return new WaitForSeconds(stats.blinkInterval);
            timer += stats.blinkInterval;
        }

        SetAlpha(1f);
        isInvincible = false;
    }

    private void SetAlpha(float alpha)
    {
        if (spriteRenderer == null) return;

        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }
}