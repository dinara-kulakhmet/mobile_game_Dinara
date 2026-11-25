using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] LevelData[] levels;
    [SerializeField] Button[] levelButtons;
    [SerializeField] GameObject[] lockIcons;
    [SerializeField] TMP_Text bestScoreText;
    [SerializeField] GameObject confirmPanel;
    [SerializeField] MainMenu mainMenu;

    void OnEnable()
    {
        if (confirmPanel != null) confirmPanel.SetActive(false);
        Refresh();
    }

    void Refresh()
    {
        for (int i = 0; i < levelButtons.Length && i < levels.Length; i++)
        {
            LevelData data = levels[i];
            bool open = SaveManager.IsUnlocked(data.levelNumber);

            levelButtons[i].interactable = open;
            if (lockIcons != null && i < lockIcons.Length && lockIcons[i] != null)
                lockIcons[i].SetActive(!open);

            levelButtons[i].onClick.RemoveAllListeners();
            levelButtons[i].onClick.AddListener(() => mainMenu.StartLevel(data));
        }

        if (bestScoreText != null)
            bestScoreText.text = "Best score: " + SaveManager.Data.bestScore;
    }

    public void OnResetPressed() { if (confirmPanel != null) confirmPanel.SetActive(true); }
    public void OnResetNo() { if (confirmPanel != null) confirmPanel.SetActive(false); }

    public void OnResetYes()
    {
        SaveManager.ResetProgress();
        if (confirmPanel != null) confirmPanel.SetActive(false);
        Refresh();
    }
}