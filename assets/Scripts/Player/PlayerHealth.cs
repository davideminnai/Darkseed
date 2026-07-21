using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 3;
    
    [System.NonSerialized] public int currentHealth;
    [System.NonSerialized] public bool isAlive = true;

    private PlayerController playerController;

    private void Awake()
    {
        currentHealth = maxHealth;
        playerController = GetComponent<PlayerController>();
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive) return;

        currentHealth -= damage;
        
        // TODO: Play hit sound and particles
        
        Debug.Log($"Player took {damage} damage. Health: {currentHealth}/{maxHealth}");
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (!isAlive) return;

        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        
        // TODO: Play heal sound and particles
        
        Debug.Log($"Player healed {amount}. Health: {currentHealth}/{maxHealth}");
    }

    private void Die()
    {
        isAlive = false;
        
        // TODO: Play death animation/sound
        // TODO: Spawn death particles
        
        Debug.Log("Player died!");
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isAlive = true;
    }
}