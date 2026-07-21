using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 8f;
    public float jumpForce = 15f;
    public float doubleJumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Invincibility")]
    public float invincibilityDuration = 2f;
    private bool isInvincible;

    [System.NonSerialized] public Rigidbody2D rb;
    [System.NonSerialized] public bool isGrounded;
    [System.NonSerialized] public bool canDoubleJump = true;
    [System.NonSerialized] public int currentLives;

    private Animator animator;
    private PlayerHealth playerHealth;
    private PlayerShooter shooter;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
        shooter = GetComponent<PlayerShooter>();
        
        currentLives = GameManager.Instance != null ? GameManager.Instance.lives : 3;
    }

    private void Update()
    {
        HandleMovement();
        HandleJumping();
        HandleShooting();
        CheckGround();
        UpdateAnimation();
        
        if (isInvincible && Time.time > invincibilityTimer)
        {
            isInvincible = false;
        }
    }

    private float invincibilityTimer;

    private void HandleMovement()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        if (moveInput > 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput < 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    private void HandleJumping()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.velocity = Vector2.up * jumpForce;
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                rb.velocity = Vector2.up * doubleJumpForce;
                canDoubleJump = false;
                // TODO: Add double jump particle effect
            }
        }
    }

    private void HandleShooting()
    {
        if (shooter != null)
        {
            shooter.HandleShooting();
        }
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        
        if (isGrounded)
        {
            canDoubleJump = true;
        }
    }

    private void UpdateAnimation()
    {
        if (animator == null) return;

        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("velocityX", Mathf.Abs(rb.velocity.x) / moveSpeed);
        animator.SetBool("isJumping", !isGrounded);
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || playerHealth == null) return;

        playerHealth.TakeDamage(damage);
        
        if (!playerHealth.isAlive)
        {
            Die();
        }
        else
        {
            StartInvincibility();
        }
    }

    private void StartInvincibility()
    {
        isInvincible = true;
        invincibilityTimer = Time.time + invincibilityDuration;
        
        // Flash effect
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            StartCoroutine(FlashEffect(renderer));
        }
    }

    private System.Collections.IEnumerator FlashEffect(SpriteRenderer renderer)
    {
        for (int i = 0; i < 10; i++)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        renderer.enabled = true;
    }

    private void Die()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoseLife();
            
            if (!GameManager.Instance.isGameOver)
            {
                Respawn();
            }
        }
        
        Destroy(gameObject);
    }

    private void Respawn()
    {
        if (GameManager.Instance != null && GameManager.Instance.spawnPoint != null)
        {
            Instantiate(GetComponent<PlayerController>().gameObject, 
                       GameManager.Instance.spawnPoint.position, 
                       Quaternion.identity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            TakeDamage(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Collectible"))
        {
            Collectible collectible = other.GetComponent<Collectible>();
            if (collectible != null)
            {
                collectible.OnCollected();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}