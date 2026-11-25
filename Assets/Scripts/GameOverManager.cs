using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private PlayerStats playerStats;

    [Header("UI")]
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private GameObject gameOverScreen;

    private int currentLives;
    private bool isGameOver;

    private void Start()
    {
        if (playerStats == null)
        {
            Debug.LogError("GameOverManager: PlayerStats is not assigned", this);
            return;
        }

        currentLives = playerStats.startingLives;
        UpdateLivesUI();

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }
    }

    public void ReduceLives()
    {
        if (isGameOver) return;

        currentLives--;

        UpdateLivesUI();

        if (currentLives <= 0)
        {
            GameOver();
        }
        else if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayDeathSound();   
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
        {
            livesText.text = "Lives: " + currentLives;
        }
    }

    private void GameOver()
    {
        isGameOver = true;

        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayLoseSound();

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        Time.timeScale = 0f;
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetPaused(true);
    }

    public void RestartLevel()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }

        ResetLives();

        if (PointsManager.Instance != null)
        {
            PointsManager.Instance.ResetScore();
        }

        Time.timeScale = 1f;

        if (SoundManager.Instance != null)
            SoundManager.Instance.SetPaused(false); 

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetLives()
    {
        currentLives = playerStats.startingLives;
        isGameOver = false;
        UpdateLivesUI();
    }
}