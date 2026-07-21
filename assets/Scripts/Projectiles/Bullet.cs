using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float lifetime = 3f; // Seconds before bullet disappears
    
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool isPlayerBullet;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Initialize(Vector2 direction, bool isFromPlayer = true)
    {
        moveDirection = direction.normalized;
        isPlayerBullet = isFromPlayer;
        
        rb.velocity = moveDirection * 15f; // Bullet speed
        
        // Set rotation to face movement direction
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // Destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayerBullet)
        {
            // Player bullet hits enemy or hazard
            if (other.CompareTag("Enemy") || other.CompareTag("Hazard"))
            {
                EnemyHealth enemy = other.GetComponent<EnemyHealth>();
                if (enemy != null)
                {
                    enemy.TakeDamage(1);
                }
                
                Destroy(gameObject);
            }
        }
        else
        {
            // Enemy bullet hits player
            if (other.CompareTag("Player"))
            {
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.TakeDamage(1);
                }
                
                Destroy(gameObject);
            }
        }
        
        // Bullet destroyed on any wall/platform collision
        if (other.CompareTag("Ground") || other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}