using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))]
public class FinalBoss : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private GameObject healthBarRoot;

    [Header("Activation")]
    [SerializeField] private float activationRange = 12f;

    [Header("Visuals")]
    [SerializeField] private bool spriteFacesLeft = true;
    [SerializeField] private float hoverHeight = 0.25f;
    [SerializeField] private float hoverSpeed = 2f;

    [Header("Attacks")]
    [SerializeField] private BossProjectile projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 6f;
    [SerializeField] private float attackInterval = 2.5f;
    [SerializeField] private float phaseTwoAttackInterval = 1.5f;

    [Header("Fan attack (shots at the player)")]
    [SerializeField] private int fanProjectiles = 3;
    [SerializeField] private int phaseTwoFanProjectiles = 5;
    [SerializeField] private float fanAngle = 18f;

    [Header("Rain attack (falls from above)")]
    [SerializeField] private int rainProjectiles = 4;
    [SerializeField] private int phaseTwoRainProjectiles = 7;
    [SerializeField] private float rainSpread = 5f;
    [SerializeField] private float rainSpawnHeight = 7f;
    [SerializeField] private float rainWarningTime = 0.8f;
    [SerializeField] private GameObject rainWarningPrefab;

    [Header("When defeated")]
    [SerializeField] private WinScreen winScreen;

    private const float RainWallIgnoreTime = 0.6f;

    private int currentHealth;
    private bool isActive;
    private bool isDead;
    private bool nextAttackIsFan = true;
    private Vector3 startPosition;
    private SpriteRenderer spriteRenderer;
    private Color originalColor = Color.white;
    private PlayerController player;
    private Coroutine flashRoutine;
    
    private bool IsPhaseTwo => currentHealth <= maxHealth / 2;

    private void Awake()
    {
        currentHealth = maxHealth;
        startPosition = transform.position;

        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        if (healthBarRoot != null)
        {
            healthBarRoot.SetActive(false);
        }

        UpdateHealthBar();
    }

    private IEnumerator Start()
    {
        player = FindAnyObjectByType<PlayerController>();

        yield return null;

        ResolveHealthBar();

        if (!isActive && healthBarRoot != null)
        {
            healthBarRoot.SetActive(false);
        }

        UpdateHealthBar();
    }

    private void Update()
    {
        if (isDead) return;

        if (player == null)
        {
            player = FindAnyObjectByType<PlayerController>();
            return;
        }

        if (!isActive)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance <= activationRange)
            {
                Activate();
            }
            return;
        }

        Hover();
        FacePlayer();
    }

    private void Activate()
    {
        if (isActive || isDead) return;

        isActive = true;

        ResolveHealthBar();
        UpdateHealthBar();

        if (healthBarRoot != null)
        {
            healthBarRoot.SetActive(true);
        }

        if (projectilePrefab == null)
        {
            Debug.LogWarning("FinalBoss: Projectile Prefab is not assigned, the boss will not attack", this);
        }

        StartCoroutine(AttackLoop());
    }

    private void Hover()
    {
        float offset = (Mathf.Sin(Time.time * hoverSpeed) + 1f) * 0.5f * hoverHeight;
        transform.position = startPosition + Vector3.up * offset;
    }

    private void FacePlayer()
    {
        if (spriteRenderer == null) return;

        bool playerIsRight = player.transform.position.x > transform.position.x;
        spriteRenderer.flipX = playerIsRight ? spriteFacesLeft : !spriteFacesLeft;
    }


    private IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(1f);

        while (!isDead)
        {
            if (player != null && projectilePrefab != null)
            {
                if (nextAttackIsFan)
                {
                    FireFan();
                }
                else
                {
                    yield return StartCoroutine(RainAttack());
                }

                nextAttackIsFan = !nextAttackIsFan;
            }

            yield return new WaitForSeconds(IsPhaseTwo ? phaseTwoAttackInterval : attackInterval);
        }
    }

    private void FireFan()
    {
        int count = IsPhaseTwo ? phaseTwoFanProjectiles : fanProjectiles;
        Vector3 origin = firePoint != null ? firePoint.position : transform.position;

        Vector2 toPlayer = (player.transform.position - origin).normalized;
        float baseAngle = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        for (int i = 0; i < count; i++)
        {
            float offset = (i - (count - 1) / 2f) * fanAngle;
            float angle = (baseAngle + offset) * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            SpawnProjectile(origin, direction, 0f);
        }
    }

    private IEnumerator RainAttack()
    {
        int count = IsPhaseTwo ? phaseTwoRainProjectiles : rainProjectiles;
        float[] columns = new float[count];
        float groundY = player.transform.position.y;

        for (int i = 0; i < count; i++)
        {
            columns[i] = player.transform.position.x + Random.Range(-rainSpread, rainSpread);

            if (rainWarningPrefab != null)
            {
                GameObject warning = Instantiate(rainWarningPrefab, new Vector3(columns[i], groundY, 0f), Quaternion.identity);
                Destroy(warning, rainWarningTime);
            }
        }

        yield return new WaitForSeconds(rainWarningTime);

        for (int i = 0; i < count; i++)
        {
            Vector3 position = new Vector3(columns[i], groundY + rainSpawnHeight, 0f);
            SpawnProjectile(position, Vector2.down, RainWallIgnoreTime);
        }
    }

    private void SpawnProjectile(Vector3 position, Vector2 direction, float ignoreWallsFor)
    {
        BossProjectile projectile = Instantiate(projectilePrefab, position, Quaternion.identity);
        projectile.Launch(direction, projectileSpeed, ignoreWallsFor);
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        // Shot from far away: the fight starts anyway
        Activate();

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        UpdateHealthBar();

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBossHitSound();
        }

        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashRoutine());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDead) return;

        PlayerHealth health = collision.collider.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage();
        }
    }

    private IEnumerator FlashRoutine()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
        }

        yield return new WaitForSeconds(0.1f);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }

    private void ResolveHealthBar()
    {
        if (healthSlider != null && healthBarRoot != null) return;

        foreach (Slider s in Resources.FindObjectsOfTypeAll<Slider>())
        {
            if (s.gameObject.scene.IsValid() && s.gameObject.name == "BossHealthBar")
            {
                healthSlider = s;
                healthBarRoot = s.gameObject;
                break;
            }
        }
    }

    private void UpdateHealthBar()
    {
        if (healthSlider != null)
        {
            healthSlider.value = (float)currentHealth / maxHealth;
        }
    }

    private void Die()
    {
        isDead = true;

        StopAllCoroutines();

        // Remove everything that is still flying
        foreach (BossProjectile projectile in FindObjectsByType<BossProjectile>())
        {
            Destroy(projectile.gameObject);
        }

        foreach (Collider2D col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        if (healthBarRoot != null)
        {
            healthBarRoot.SetActive(false);
        }

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        const float duration = 1f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            if (spriteRenderer != null)
            {
                Color color = originalColor;
                color.a = 1f - timer / duration;
                spriteRenderer.color = color;
            }

            yield return null;
        }

        if (winScreen == null)
        {
            foreach (WinScreen w in Resources.FindObjectsOfTypeAll<WinScreen>())
            {
                if (w.gameObject.scene.IsValid())
                {
                    winScreen = w;
                    break;
                }
            }
        }

        if (winScreen != null)
        {
            winScreen.Show();
        }
        else
        {
            Debug.Log("Boss defeated! WinScreen not found in any loaded scene.", this);
        }

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (healthBarRoot != null)
        {
            healthBarRoot.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, activationRange);
    }
}