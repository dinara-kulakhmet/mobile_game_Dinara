using UnityEngine;

public class ChargerEnemy : EnemyBase
{
    private enum State { Idle, WindUp, Charge, Recover }

    [Header("Data")]
    [SerializeField] private ChargerStats stats;

    protected override EnemyStats BaseStats => stats;

    private State state = State.Idle;
    private float timer;
    private int chargeDir = 1;
    private float startX;
    private bool hitDone;

    private Collider2D myCollider;
    private Collider2D playerCollider;

    protected override void Awake()
    {
        base.Awake();
        myCollider = GetComponent<Collider2D>();
    }

    protected override void Start()
    {
        base.Start();
        if (player != null) playerCollider = player.GetComponentInChildren<Collider2D>();
    }

    private bool TouchesPlayer()
    {
        if (myCollider == null || playerCollider == null)
            return DistanceToPlayer() <= stats.hitRadius;

        Bounds b = myCollider.bounds;
        b.Expand(stats.hitPadding);
        return b.Intersects(playerCollider.bounds);
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:    UpdateIdle();    break;
            case State.WindUp:  UpdateWindUp();  break;
            case State.Charge:  UpdateCharge();  break;
            case State.Recover: UpdateRecover(); break;
        }
    }

    private void UpdateIdle()
    {
        if (!CanSeePlayer() || !CanAttackNow()) return;

        chargeDir = player.position.x > transform.position.x ? 1 : -1;
        SetFacing(chargeDir);
        timer = 0f;
        state = State.WindUp;
        SetTelegraph(true);
    }

    private void UpdateWindUp()
    {
        timer += Time.deltaTime;
        if (timer >= stats.windUp)
        {
            SetTelegraph(false);
            startX = transform.position.x;
            hitDone = false;
            state = State.Charge;
        }
    }

    private void UpdateCharge()
    {
        RaycastHit2D wall = Physics2D.Raycast(transform.position, new Vector2(chargeDir, 0f),
                                              stats.wallCheckDistance, obstacleMask);
        bool tooFar = Mathf.Abs(transform.position.x - startX) >= stats.maxChargeDistance;

        Vector2 probe = (Vector2)transform.position + new Vector2(chargeDir * stats.edgeProbeOffset, 0f);
        bool noGround = Physics2D.Raycast(probe, Vector2.down, stats.edgeProbeDepth, obstacleMask).collider == null;

        if (wall.collider != null || tooFar || noGround)
        {
            timer = 0f;
            state = State.Recover;
            return;
        }

        transform.position += new Vector3(chargeDir * stats.chargeSpeed * Time.deltaTime, 0f, 0f);

        if (!hitDone && TouchesPlayer())
        {
            hitDone = true;
            DealDamage(player.gameObject, stats.damage);
        }
    }

    private void UpdateRecover()
    {
        timer += Time.deltaTime;
        if (timer >= stats.recoverTime) state = State.Idle;
    }
}