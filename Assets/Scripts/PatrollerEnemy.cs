using UnityEngine;

public class PatrollerEnemy : EnemyBase
{
    private enum State { Patrol, Chase, Attack }

    [Header("Data")]
    [SerializeField] private PatrollerStats stats;

    [Header("Patrol")]
    [SerializeField] private Transform[] waypoints;

    protected override EnemyStats BaseStats => stats;

    private State state = State.Patrol;
    private int waypointIndex;
    private float waitTimer;
    private float lostTimer;
    private float attackTimer;
    private bool struck;
    private float lastContactHit = float.NegativeInfinity;

    private void Update()
    {
        switch (state)
        {
            case State.Patrol: UpdatePatrol(); break;
            case State.Chase:  UpdateChase();  break;
            case State.Attack: UpdateAttack(); break;
        }
    }

    private void UpdatePatrol()
    {
        if (CanSeePlayer())
        {
            state = State.Chase;
            lostTimer = 0f;
            return;
        }

        if (waypoints == null || waypoints.Length == 0) return;

        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        float targetX = waypoints[waypointIndex].position.x;
        bool moved = MoveTowardsX(targetX, stats.patrolSpeed);

        if (Mathf.Abs(transform.position.x - targetX) < 0.05f || !moved)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
            waitTimer = stats.waypointWait;
        }
    }

    private void UpdateChase()
    {
        if (CanSeePlayer()) lostTimer = 0f;
        else lostTimer += Time.deltaTime;

        if (lostTimer > stats.loseSightTime)
        {
            state = State.Patrol;
            return;
        }

        if (player == null) return;

        if (DistanceToPlayer() <= stats.attackRange && CanAttackNow())
        {
            state = State.Attack;
            attackTimer = 0f;
            struck = false;
            SetTelegraph(true);
            PlayWindupSound();  
            return;
        }

        MoveTowardsX(player.position.x, stats.chaseSpeed);
    }

    private void UpdateAttack()
    {
        attackTimer += Time.deltaTime;

        if (!struck && attackTimer >= stats.windUp)
        {
            struck = true;
            SetTelegraph(false);
            if (DistanceToPlayer() <= stats.attackRange)
                DealDamage(player.gameObject, stats.damage);
        }

        if (struck && attackTimer >= stats.windUp + stats.attackCooldown)
            state = State.Chase;
    }

    private void OnCollisionStay2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Player") && Time.time - lastContactHit >= stats.contactCooldown)
        {
            lastContactHit = Time.time;
            DealDamage(c.gameObject, stats.damage);
        }
    }

    private bool HasGroundAhead(int dir)
    {
        Vector2 origin = (Vector2)transform.position + new Vector2(dir * stats.edgeProbeOffset, 0f);
        return Physics2D.Raycast(origin, Vector2.down, stats.edgeProbeDepth, obstacleMask).collider != null;
    }

    private bool MoveTowardsX(float targetX, float speed)
    {
        float dx = targetX - transform.position.x;
        if (Mathf.Abs(dx) <= 0.01f) return false;

        int dir = dx > 0f ? 1 : -1;
        SetFacing(dir);

        if (!HasGroundAhead(dir)) return false;

        Vector3 p = transform.position;
        p.x = Mathf.MoveTowards(p.x, targetX, speed * Time.deltaTime);
        transform.position = p;
        return true;
    }
}