using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject UI;

    private void Start()
    {
        // The game can be paused (timeScale = 0) when the player comes back to the menu
        Time.timeScale = 1f;
    }

    public void PlayGame()
    {
        LoadLevel(1);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    public void OpenLevelSelect()
    {
        mainMenuPanel.SetActive(false);
        levelSelectPanel.SetActive(true);
    }

    public void CloseLevelSelect()
    {
        mainMenuPanel.SetActive(true);
        levelSelectPanel.SetActive(false);
    }

    public void LoadLevel(int levelIndex)
    {
        EnableUI();
        SceneManager.LoadScene(levelIndex);
    }

    public void EnableUI()
    {
        // After the first game the UI already exists (DontDestroyOnLoad),
        // the copy in this scene is not the one that must be shown
        GameObject ui = PersistentUI.Instance != null ? PersistentUI.Instance.gameObject : UI;

        if (ui != null)
        {
            ui.SetActive(true);
        }
    }
}
