using UnityEngine;

public class ChargerEnemy : EnemyBase
{
    private enum State { Idle, WindUp, Charge, Recover }

    [Header("Charge")]
    [SerializeField] private float windUp = 0.7f;            
    [SerializeField] private float chargeSpeed = 9f;
    [SerializeField] private float maxChargeDistance = 6f;
    [SerializeField] private float wallCheckDistance = 0.6f;
    [SerializeField] private float recoverTime = 1.2f;       
    [SerializeField] private float hitRadius = 0.6f;
    [SerializeField] private int damage = 1;

    [Header("Edge check")]
    [SerializeField] private float edgeProbeOffset = 0.6f;
    [SerializeField] private float edgeProbeDepth = 1.5f;

    private State state = State.Idle;
    private float timer;
    private int chargeDir = 1;
    private float startX;
    private bool hitDone;

    [Header("Hit detection")]
    [SerializeField] private float hitPadding = 0.1f; 
    private Collider2D myCollider;
    private Collider2D playerCollider;

    private void Reset()
    {
        visionAngle = 140f;
    }

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
            return DistanceToPlayer() <= hitRadius; 

        Bounds b = myCollider.bounds;
        b.Expand(hitPadding);
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
        if (timer >= windUp)
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
                                              wallCheckDistance, obstacleMask);
        bool tooFar = Mathf.Abs(transform.position.x - startX) >= maxChargeDistance;

        Vector2 probe = (Vector2)transform.position + new Vector2(chargeDir * edgeProbeOffset, 0f);
        bool noGround = Physics2D.Raycast(probe, Vector2.down, edgeProbeDepth, obstacleMask).collider == null;

        if (wall.collider != null || tooFar || noGround)
        {
            timer = 0f;
            state = State.Recover;
            return;
        }

        transform.position += new Vector3(chargeDir * chargeSpeed * Time.deltaTime, 0f, 0f);

        if (!hitDone && TouchesPlayer())
        {
            hitDone = true;
            DealDamage(player.gameObject, damage);
        }
    }

    private void UpdateRecover()
    {
        timer += Time.deltaTime;
        if (timer >= recoverTime) state = State.Idle;
    }
}