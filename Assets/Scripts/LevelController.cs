using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] LevelData fallback;          
    [SerializeField] Transform player;
    [SerializeField] Transform playerSpawnPoint;  
    [SerializeField] Transform goal;              
    [SerializeField] string exitSceneName = "MainMenu";

    public float TimeLeft { get; private set; }
    bool finished;

    void Start()
    {
        LevelData data = GameSession.Current != null ? GameSession.Current : fallback;
        if (data == null)
        {
            Debug.LogError("LevelController: no LevelData assigned");
            return;
        }
        GameSession.Current = data;
        GameSession.Score = 0;

        if (data.layoutPrefab != null) Instantiate(data.layoutPrefab);

        if (player != null) player.position = data.playerSpawn;
        if (playerSpawnPoint != null) playerSpawnPoint.position = data.playerSpawn;

        if (goal != null)
        {
            goal.gameObject.SetActive(data.hasGoal);
            if (data.hasGoal) goal.position = data.goalPosition;
        }

        foreach (SpawnEntry e in data.enemies)
            if (e.prefab != null) Instantiate(e.prefab, e.position, Quaternion.identity);
        foreach (SpawnEntry i in data.items)
            if (i.prefab != null) Instantiate(i.prefab, i.position, Quaternion.identity);

        TimeLeft = data.timeLimit;
    }

    void Update()
    {
        if (finished || GameSession.Current == null) return;
        TimeLeft -= Time.deltaTime;
        if (TimeLeft <= 0f) Lose();
    }

    public void CompleteLevel()
    {
        if (finished) return;
        finished = true;
        SaveManager.CompleteLevel(GameSession.Current.levelNumber, GameSession.Score);
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene(exitSceneName);
    }

    public void Win()
    {
        CompleteLevel();
        ExitToMenu();
    }

    public void Lose()
    {
        if (finished) return;
        finished = true;
        ExitToMenu();
    }
}