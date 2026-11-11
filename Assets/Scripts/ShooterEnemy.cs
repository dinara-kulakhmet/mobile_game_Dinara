using UnityEngine;

/// <summary>
/// Враг 2: Shooter. Стоит на месте, видит игрока через Raycast, прицеливается
/// (wind-up с телеграфом), стреляет, ждёт cooldown.
/// States: Idle -> Aim -> Fire -> Cooldown -> Idle
/// </summary>
public class ShooterEnemy : EnemyBase
{
    private enum State { Idle, Aim, Fire, Cooldown }

    [Header("Shooting")]
    [SerializeField] private EnemyProjectile projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float aimTime = 0.6f;     // wind-up 0.3-0.8 s
    [SerializeField] private float cooldown = 1.6f;

    [Header("Optional aim line (LineRenderer)")]
    [SerializeField] private LineRenderer aimLine;

    private State state = State.Idle;
    private float timer;

    private void Reset()
    {
        visionAngle = 360f; // стоит на месте — видит во все стороны
    }

    protected override void Awake()
    {
        base.Awake();
        if (aimLine != null) aimLine.enabled = false;
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:     UpdateIdle();     break;
            case State.Aim:      UpdateAim();      break;
            case State.Fire:     FireProjectile(); break;
            case State.Cooldown: UpdateCooldown(); break;
        }
    }

    private void UpdateIdle()
    {
        if (CanSeePlayer() && CanAttackNow())
        {
            state = State.Aim;
            timer = 0f;
            SetTelegraph(true);
            if (aimLine != null) aimLine.enabled = true;
        }
    }

    private void UpdateAim()
    {
        // Потерял игрока за стеной — отменяем выстрел (честно)
        if (!CanSeePlayer())
        {
            StopAiming();
            state = State.Idle;
            return;
        }

        SetFacing(player.position.x > transform.position.x ? 1 : -1);

        if (aimLine != null)
        {
            Vector3 origin = firePoint != null ? firePoint.position : transform.position;
            aimLine.SetPosition(0, origin);
            aimLine.SetPosition(1, player.position);
        }

        timer += Time.deltaTime;
        if (timer >= aimTime) state = State.Fire;
    }

    private void FireProjectile()
    {
        StopAiming();

        if (projectilePrefab != null && player != null)
        {
            Vector3 origin = firePoint != null ? firePoint.position : transform.position;
            Vector2 dir = (player.position - origin).normalized;
            EnemyProjectile p = Instantiate(projectilePrefab, origin, Quaternion.identity);
            p.Launch(dir);
        }

        timer = 0f;
        state = State.Cooldown;
    }

    private void UpdateCooldown()
    {
        timer += Time.deltaTime;
        if (timer >= cooldown) state = State.Idle;
    }

    private void StopAiming()
    {
        SetTelegraph(false);
        if (aimLine != null) aimLine.enabled = false;
    }
}
