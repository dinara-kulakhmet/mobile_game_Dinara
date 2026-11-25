using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject levelSelectPanel;
    [SerializeField] private GameObject UI;
    [SerializeField] private LevelData[] levels;         
    [SerializeField] private string gameSceneName = "Game";

    private void Start()
    {
        Time.timeScale = 1f;
    }

    public void PlayGame()
    {
        if (levels == null || levels.Length == 0) return;

        LevelData target = levels[0];
        foreach (LevelData l in levels)
            if (SaveManager.IsUnlocked(l.levelNumber)) target = l;

        StartLevel(target);
    }

    public void StartLevel(LevelData data)
    {
        if (data == null) return;
        GameSession.Current = data;
        EnableUI();
        SceneManager.LoadScene(gameSceneName);
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

    public void EnableUI()
    {
        GameObject ui = PersistentUI.Instance != null ? PersistentUI.Instance.gameObject : UI;
        if (ui != null) ui.SetActive(true);
    }
}