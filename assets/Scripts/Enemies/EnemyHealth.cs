using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    
    [System.NonSerialized] public int currentHealth;
    [System.NonSerialized] public bool isAlive = true;
    [System.NonSerialized] public bool isBoss;

    private EnemyController enemyController;
    private GameObject scoreManager;

    private void Awake()
    {
        currentHealth = maxHealth;
        enemyController = GetComponent<EnemyController>();
        
        if (gameObject.CompareTag("Boss"))
        {
            isBoss = true;
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive) return;

        currentHealth -= damage;
        
        // TODO: Play hit sound and particles
        // Flash effect
        var renderer = GetComponent<SpriteRenderer>();
        if (renderer != null)
        {
            StartCoroutine(FlashEffect(renderer));
        }
        
        Debug.Log($"{gameObject.name} took {damage} damage. Health: {currentHealth}/{maxHealth}");
        
        if (enemyController != null)
        {
            enemyController.OnHit(damage);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private System.Collections.IEnumerator FlashEffect(SpriteRenderer renderer)
    {
        for (int i = 0; i < 5; i++)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(0.05f);
        }
        renderer.enabled = true;
    }

    private void Die()
    {
        isAlive = false;
        
        // TODO: Play death animation/sound
        // Spawn particles
        
        Debug.Log($"{gameObject.name} died!");
        
        if (isBoss)
        {
            // Boss specific death behavior
            OnBossDeath();
        }
        
        // Add score
        int scoreValue = isBoss ? 500 : 100;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(scoreValue);
        }
        
        Destroy(gameObject, 2f); // Delay destruction for death animation
    }

    private void OnBossDeath()
    {
        // TODO: Spawn explosion particles
        // TODO: Play special boss death sound
        // TODO: Trigger victory condition if needed
        
        Debug.Log("BOSS DEFEATED!");
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddScore(500);
        }
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isAlive = true;
    }
}