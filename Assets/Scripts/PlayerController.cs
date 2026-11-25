using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private PlayerStats stats;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public PlayerStats Stats => stats;

    private int jumpsRemaining;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rigidBody;
    private bool isGrounded;
    private Animator playerAnimator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        playerControls = new PlayerControls();
        rigidBody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (GetComponent<PlayerHealth>() == null)
        {
            gameObject.AddComponent<PlayerHealth>();
        }
    }

    private void OnEnable()
    {
        playerControls.Movement.Jump.performed += OnJump;
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Movement.Jump.performed -= OnJump;
        playerControls.Disable();
    }

    private void OnDestroy()
    {
        playerControls.Dispose();
    }

    private void Update()
    {
        PlayerInput();
        CheckIsGrounded();
        UpdateAnimations();
        FlipSprite();
        ResetJumps();
    }

    private void FixedUpdate()
    {
        Move();
        HandleBetterFall();
        HandleVariableJump();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();
    }

    private void CheckIsGrounded()
    {
        if (groundCheck == null)
        {
            isGrounded = false;
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, stats.groundCheckRadius, groundLayer);
    }

    private void Move()
    {
        rigidBody.linearVelocity = new Vector2(movement.x * stats.moveSpeed, rigidBody.linearVelocity.y);
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (Time.timeScale == 0f) return;
        if (jumpsRemaining <= 0) return;

        if (!isGrounded && stats.amountOfJumps > 1 && playerAnimator != null)
        {
            playerAnimator.SetTrigger("doubleJump");
        }

        rigidBody.linearVelocity = new Vector2(rigidBody.linearVelocity.x, stats.jumpForce);
        jumpsRemaining--;

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayJumpSound();
        }
    }

    private void ResetJumps()
    {
        if (isGrounded && rigidBody.linearVelocity.y <= 0.01f)
        {
            jumpsRemaining = stats.amountOfJumps;
        }
    }

    private void HandleBetterFall()
    {
        if (rigidBody.linearVelocity.y < 0)
        {
            rigidBody.linearVelocity += Vector2.up * Physics2D.gravity.y * stats.fallMultiplier * Time.fixedDeltaTime;
        }
    }

    private void HandleVariableJump()
    {
        bool releasedJumpEarly = rigidBody.linearVelocity.y > 0 && !playerControls.Movement.Jump.IsPressed();

        if (releasedJumpEarly)
        {
            rigidBody.linearVelocity += Vector2.up * Physics2D.gravity.y * stats.lowJumpMultiplier * Time.fixedDeltaTime;
        }
    }

    private void UpdateAnimations()
    {
        if (playerAnimator == null) return;

        playerAnimator.SetFloat("moveX", movement.x);
        playerAnimator.SetBool("isGrounded", isGrounded);
    }

    private void FlipSprite()
    {
        if (spriteRenderer == null) return;

        if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }
}