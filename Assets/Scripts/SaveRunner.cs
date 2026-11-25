using UnityEngine;

// Кладётся на пустой объект SaveRunner в ПЕРВОЙ сцене (MainMenu).
// Загружает сохранение и сохраняет при сворачивании и выходе.
public class SaveRunner : MonoBehaviour
{
    static SaveRunner instance;

    void Awake()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SaveManager.Load();
    }

    void OnApplicationPause(bool paused) { if (paused) SaveManager.Save(); }
    void OnApplicationQuit() { SaveManager.Save(); }
}
