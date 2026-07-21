using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Game Settings")]
    public int maxLives = 3;
    public int scoreToWin = 1000;

    [Header("References")]
    public GameObject playerPrefab;
    public Transform spawnPoint;

    [System.NonSerialized] public int currentScore;
    [System.NonSerialized] public int lives;
    [System.NonSerialized] public bool isGameOver;
    [System.NonSerialized] public bool isVictory;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeGame();
    }

    public void InitializeGame()
    {
        currentScore = 0;
        lives = maxLives;
        isGameOver = false;
        isVictory = false;

        if (spawnPoint != null && playerPrefab != null)
        {
            Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    public void AddScore(int points)
    {
        currentScore += points;
        
        if (currentScore >= scoreToWin && !isVictory)
        {
            Victory();
        }
    }

    public void LoseLife()
    {
        lives--;
        
        if (lives <= 0)
        {
            GameOver();
        }
    }

    public void RespawnPlayer()
    {
        if (spawnPoint != null && playerPrefab != null)
        {
            Instantiate(playerPrefab, spawnPoint.position, Quaternion.identity);
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER!");
        // TODO: Show game over UI
    }

    private void Victory()
    {
        isVictory = true;
        Debug.Log("VICTORY!");
        // TODO: Show victory UI
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}