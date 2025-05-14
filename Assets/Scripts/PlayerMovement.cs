using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Movement")]
    public float moveSpeed = 5f;
    public float runSpeed = 8f;
    public float jumpForce = 10f;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;
    private PlayerController playerController;

    private Vector2 moveInput;
    private bool isJumping = false;

    private enum MovementState { idle, walk, jump, fall, run }

    [Header("Jump Settings")]
    [SerializeField] private LayerMask jumpableGround;
    private BoxCollider2D coll;

    [Header("Attack Settings")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public GameObject projectilePrefab;
    public Transform throwPoint;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();

        playerController = new PlayerController();
    }

    private void OnEnable()
    {
        playerController.Enable();

        playerController.Movement.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerController.Movement.Move.canceled += ctx => moveInput = Vector2.zero;

        playerController.Movement.Jump.performed += ctx => Jump();
        playerController.Movement.Hit.performed += ctx => MeleeAttack();
        playerController.Movement.Throw.performed += ctx => ThrowProjectile();
    }

    private void OnDisable()
    {
        playerController.Disable();
    }

    private void Update()
    {
        moveInput = playerController.Movement.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        Vector2 targetVelocity = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
        rb.velocity = targetVelocity;
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        MovementState state;

        if (moveInput.x > 0f)
        {
            state = MovementState.walk;
            sprite.flipX = false;
        }
        else if (moveInput.x < 0f)
        {
            state = MovementState.walk;
            sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > 0.1f)
        {
            state = MovementState.jump;
        }
        else if (rb.velocity.y < -0.1f)
        {
            state = MovementState.fall;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool isGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void Jump()
    {
        if (isGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void MeleeAttack()
    {
        anim.SetTrigger("attack"); // Pastikan animator punya trigger "attack"

        // Deteksi musuh di sekitar
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit: " + enemy.name);
            // enemy.GetComponent<Enemy>().TakeDamage(damage); // jika punya script Enemy
        }
    }

    private void ThrowProjectile()
    {
        anim.SetTrigger("throw"); // Pastikan animator punya trigger "throw"

        GameObject projectile = Instantiate(projectilePrefab, throwPoint.position, Quaternion.identity);
        Rigidbody2D rbProj = projectile.GetComponent<Rigidbody2D>();

        float direction = sprite.flipX ? -1f : 1f;
        rbProj.velocity = new Vector2(10f * direction, 0f);
        projectile.transform.localScale = new Vector3(direction, 1f, 1f);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
