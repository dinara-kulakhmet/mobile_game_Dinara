using UnityEngine;
using UnityEngine.InputSystem;

// Shoots a projectile in the direction the player is facing.
// Hold the button to keep shooting; Cooldown limits the fire rate.
public class PlayerAttack : MonoBehaviour
{
    [Header("Projectile")]
    [SerializeField] private PlayerProjectile projectilePrefab;
    [SerializeField] private Vector2 spawnOffset = new Vector2(0.6f, 0f);
    [SerializeField] private float projectileSpeed = 12f;

    [Header("Attack")]
    [SerializeField] private float cooldown = 0.3f;

    private InputAction attackAction;
    private SpriteRenderer spriteRenderer;
    private float nextAttackTime;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Keyboard: X. Gamepad West button is also what the on-screen FIRE button
        // must use as its Control Path: <Gamepad>/buttonWest
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
        // No shooting while the game is paused (game over / win screen)
        if (Time.timeScale == 0f) return;

        if (attackAction.IsPressed() && Time.time >= nextAttackTime)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("PlayerAttack: Projectile Prefab is not assigned", this);
            return;
        }

        nextAttackTime = Time.time + cooldown;

        // The sprite is flipped when the player runs to the left (see PlayerController)
        float direction = (spriteRenderer != null && spriteRenderer.flipX) ? -1f : 1f;

        Vector3 spawnPosition = transform.position + new Vector3(spawnOffset.x * direction, spawnOffset.y, 0f);
        PlayerProjectile projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
        projectile.Launch(new Vector2(direction, 0f), projectileSpeed);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayShootSound();
        }
    }
}
