using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("References")]
    public Text scoreText;
    public Text livesText;
    public Image[] healthBars; // Array of heart icons or health bar images
    
    [Header("Settings")]
    public Sprite fullHeart;
    public Sprite emptyHeart;

    private PlayerHealth playerHealth;
    private int previousLives = -1;

    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        UpdateHUD();
    }

    private void Update()
    {
        // Check for life changes
        if (GameManager.Instance != null && GameManager.Instance.lives != previousLives)
        {
            previousLives = GameManager.Instance.lives;
            UpdateLivesDisplay();
        }
    }

    public void UpdateHUD()
    {
        if (scoreText != null && GameManager.Instance != null)
        {
            scoreText.text = $"SCORE: {GameManager.Instance.currentScore}";
        }

        UpdateLivesDisplay();
        UpdateHealthBars();
    }

    private void UpdateLivesDisplay()
    {
        if (livesText != null && GameManager.Instance != null)
        {
            livesText.text = $"LIVES: {GameManager.Instance.lives}";
        }
    }

    private void UpdateHealthBars()
    {
        if (playerHealth == null || healthBars == null) return;

        for (int i = 0; i < healthBars.Length; i++)
        {
            if (i < playerHealth.currentHealth && fullHeart != null)
            {
                healthBars[i].sprite = fullHeart;
            }
            else if (emptyHeart != null)
            {
                healthBars[i].sprite = emptyHeart;
            }
        }
    }

    public void ShowGameOver()
    {
        // TODO: Display game over screen
        Debug.Log("Showing Game Over UI");
    }

    public void ShowVictory()
    {
        // TODO: Display victory screen
        Debug.Log("Showing Victory UI");
    }
}