using UnityEngine;
using TMPro;

public class PointsManager : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;

    public static PointsManager Instance;

    private int points = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdatePointsText();
    }

    public void IncreasePoints(int amount)
    {
        points += amount;
        UpdatePointsText();
    }

    public void ResetScore()
    {
        points = 0;
        UpdatePointsText();
    }

    private void UpdatePointsText()
    {
        if (pointsText != null)
        {
            pointsText.text = "Points: " + points;
        }
    }
}
