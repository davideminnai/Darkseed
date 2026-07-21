using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Collectible Settings")]
    public int scoreValue = 100;
    public float collectRadius = 0.5f;
    
    private Animator animator;
    private Rigidbody2D rb;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            rb.gravityScale = 0; // Disable gravity for collectibles
        }
        
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    private void Update()
    {
        // Gentle floating animation
        float newY = spawnPosition.y + Mathf.Sin(Time.time * 2) * 0.1f;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        
        // Rotate slowly
        transform.Rotate(Vector3.forward * 50 * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnCollected();
        }
    }

    public void OnCollected()
    {
        // Add score
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }
        
        // TODO: Play collect sound and particles
        
        Debug.Log($"Collectible collected! +{scoreValue} points");
        
        // Animate collection
        if (animator != null)
        {
            animator.SetTrigger("collect");
        }
        
        // Destroy after animation
        Destroy(gameObject, 0.5f);
    }

    public void Respawn()
    {
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        gameObject.SetActive(true);
        
        if (animator != null)
        {
            animator.ResetTrigger("collect");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, collectRadius);
    }
}