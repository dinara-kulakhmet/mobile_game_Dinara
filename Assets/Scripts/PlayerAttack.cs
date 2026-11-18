using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private PlayerProjectile projectilePrefab;

    private PlayerStats stats;
    private InputAction attackAction;
    private SpriteRenderer spriteRenderer;
    private float nextAttackTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        PlayerController controller = GetComponent<PlayerController>();
        if (controller != null) stats = controller.Stats;

        attackAction = new InputAction("Attack", InputActionType.Button);
        attackAction.AddBinding("<Keyboard>/x");
        attackAction.AddBinding("<Gamepad>/buttonWest");
    }

    private void OnEnable()
    {
        attackAction.Enable();
    }

    private void OnDisable()
    {
        attackAction.Disable();
    }

    private void OnDestroy()
    {
        attackAction.Dispose();
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return;

        if (attackAction.IsPressed() && Time.time >= nextAttackTime)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null || stats == null)
        {
            Debug.LogWarning("PlayerAttack: Projectile Prefab or PlayerStats is not assigned", this);
            return;
        }

        nextAttackTime = Time.time + stats.attackCooldown;

        float direction = (spriteRenderer != null && spriteRenderer.flipX) ? -1f : 1f;

        Vector2 offset = stats.projectileSpawnOffset;
        Vector3 spawnPosition = transform.position + new Vector3(offset.x * direction, offset.y, 0f);
        PlayerProjectile projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        projectile.Launch(new Vector2(direction, 0f), stats);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayShootSound();
        }
    }
}