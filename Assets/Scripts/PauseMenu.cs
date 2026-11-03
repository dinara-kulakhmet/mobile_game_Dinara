using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pausePanel;

    private void Start()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    public void Pause()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }

        Time.timeScale = 0f;
    }

    public void Resume()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    public void Restart()
    {
        Resume();

        ResetProgress();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        Resume();

        ResetProgress();

        if (PersistentUI.Instance != null)
        {
            PersistentUI.Instance.gameObject.SetActive(false);
        }

        SceneManager.LoadScene(0);
    }

    private void ResetProgress()
    {
        GameOverManager gameOverManager = FindAnyObjectByType<GameOverManager>();
        if (gameOverManager != null)
        {
            gameOverManager.ResetLives();
        }

        if (PointsManager.Instance != null)
        {
            PointsManager.Instance.ResetScore();
        }
    }
}