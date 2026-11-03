using System.Collections;
using UnityEngine;

// Handles damage to the player: loses a life, plays a sound and
// makes the player blink while he is invincible.
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float invincibilityTime = 1.5f;
    [SerializeField] private float blinkInterval = 0.1f;

    private SpriteRenderer spriteRenderer;
    private bool isInvincible;

    public bool IsInvincible => isInvincible;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnDisable()
    {
        // Do not stay half-transparent if the object was disabled while blinking
        SetAlpha(1f);
        isInvincible = false;
    }

    // Returns true if the hit was counted, false if the player is invincible
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

        // If that was the last life the game is paused, no need to blink
        if (Time.timeScale > 0f)
        {
            StartCoroutine(InvincibilityRoutine());
        }

        return true;
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        float timer = 0f;
        while (timer < invincibilityTime)
        {
            if (spriteRenderer != null)
            {
                SetAlpha(spriteRenderer.color.a > 0.9f ? 0.3f : 1f);
            }

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
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
