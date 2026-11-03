using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private int startingLives = 3;
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private GameObject gameOverScreen;

    private int currentLives;
    private bool isGameOver;

    private void Start()
    {
        currentLives = startingLives;
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

        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        Time.timeScale = 0f;
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

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetLives()
    {
        currentLives = startingLives;
        isGameOver = false;
        UpdateLivesUI();
    }
}
