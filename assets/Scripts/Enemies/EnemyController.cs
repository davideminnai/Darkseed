using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    [Header("Common Settings")]
    public float detectionRange = 5f;
    public Transform playerTransform; // Reference to player
    
    protected Rigidbody2D rb;
    protected EnemyHealth enemyHealth;
    protected Animator animator;
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyHealth = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
        
        FindPlayer();
    }

    protected virtual void Update()
    {
        if (playerTransform == null) return;
        
        CheckDetection();
        UpdateAnimation();
    }

    protected void FindPlayer()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }
    }

    protected virtual void CheckDetection()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        
        if (distanceToPlayer <= detectionRange && enemyHealth.isAlive)
        {
            OnDetectPlayer();
        }
        else
        {
            OnLostPlayer();
        }
    }

    protected abstract void OnDetectPlayer();
    protected abstract void OnLostPlayer();
    
    protected virtual void UpdateAnimation()
    {
        if (animator == null) return;
        
        animator.SetBool("isAlive", enemyHealth.isAlive);
        
        // Face player direction
        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            if (direction.x > 0.1f)
                transform.localScale = new Vector3(1, 1, 1);
            else if (direction.x < -0.1f)
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public virtual void OnHit(int damage)
    {
        // Override in subclasses for specific hit reactions
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(1);
            }
        }
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}