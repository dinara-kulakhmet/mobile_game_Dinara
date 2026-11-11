using UnityEngine;

public class PatrollerEnemy : EnemyBase
{
    private enum State { Patrol, Chase, Attack }

    [Header("Patrol")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float patrolSpeed = 1.5f;
    [SerializeField] private float waypointWait = 0.5f;

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float loseSightTime = 2f;

    [Header("Attack")]
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private float windUp = 0.5f;          
    [SerializeField] private float attackCooldown = 1.2f;
    [SerializeField] private int damage = 1;

    private State state = State.Patrol;
    private int waypointIndex;
    private float waitTimer;
    private float lostTimer;
    private float attackTimer;
    private bool struck;

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
        bool moved = MoveTowardsX(targetX, patrolSpeed);

        if (Mathf.Abs(transform.position.x - targetX) < 0.05f || !moved)
        {
            waypointIndex = (waypointIndex + 1) % waypoints.Length;
            waitTimer = waypointWait;
        }
    }

    private void UpdateChase()
    {
        if (CanSeePlayer()) lostTimer = 0f;
        else lostTimer += Time.deltaTime;

        if (lostTimer > loseSightTime)
        {
            state = State.Patrol;
            return;
        }

        if (player == null) return;

        if (DistanceToPlayer() <= attackRange && CanAttackNow())
        {
            state = State.Attack;
            attackTimer = 0f;
            struck = false;
            SetTelegraph(true); 
            return;
        }

        MoveTowardsX(player.position.x, chaseSpeed);
    }

    private void UpdateAttack()
    {
        attackTimer += Time.deltaTime;

        if (!struck && attackTimer >= windUp)
        {
            struck = true;
            SetTelegraph(false);
            if (DistanceToPlayer() <= attackRange)
                DealDamage(player.gameObject, damage);
        }

        if (struck && attackTimer >= windUp + attackCooldown)
            state = State.Chase;
    }

    [Header("Contact damage")]
    [SerializeField] private float contactCooldown = 1f;
    private float lastContactHit = -10f;

    private void OnCollisionStay2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("Player") && Time.time - lastContactHit >= contactCooldown)
        {
            lastContactHit = Time.time;
            DealDamage(c.gameObject, damage);
        }
    }

    [Header("Edge check")]
    [SerializeField] private float edgeProbeOffset = 0.6f;  
    [SerializeField] private float edgeProbeDepth = 1.5f;  

    private bool HasGroundAhead(int dir)
    {
        Vector2 origin = (Vector2)transform.position + new Vector2(dir * edgeProbeOffset, 0f);
        return Physics2D.Raycast(origin, Vector2.down, edgeProbeDepth, obstacleMask).collider != null;
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