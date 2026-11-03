using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;

    private void Start()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    public void Show()
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayWinSound();
        }

        Time.timeScale = 0f;
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        GameOverManager gameOverManager = FindAnyObjectByType<GameOverManager>();
        if (gameOverManager != null)
        {
            gameOverManager.ResetLives();
        }

        if (PointsManager.Instance != null)
        {
            PointsManager.Instance.ResetScore();
        }

        if (PersistentUI.Instance != null)
        {
            PersistentUI.Instance.gameObject.SetActive(false);
        }

        SceneManager.LoadScene(0);
    }
}