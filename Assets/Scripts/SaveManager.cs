using System;
using System.IO;
using UnityEngine;

// Загрузка и сохранение прогресса в JSON. Сломанный или пропавший файл -> значения по умолчанию.
public static class SaveManager
{
    static SaveData _data;
    static string FilePath => Path.Combine(Application.persistentDataPath, "save.json");

    public static SaveData Data
    {
        get { if (_data == null) Load(); return _data; }
    }

    public static void Load()
    {
        try
        {
            if (File.Exists(FilePath))
            {
                SaveData d = JsonUtility.FromJson<SaveData>(File.ReadAllText(FilePath));
                if (d != null && d.version == 1 && d.unlocked != null && d.unlocked.Count > 0)
                {
                    _data = d;
                    return;
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Save is broken, using defaults: " + e.Message);
        }
        _data = new SaveData();
    }

    public static void Save()
    {
        try
        {
            string tmp = FilePath + ".tmp";
            File.WriteAllText(tmp, JsonUtility.ToJson(Data, true));
            if (File.Exists(FilePath)) File.Delete(FilePath);
            File.Move(tmp, FilePath);
        }
        catch (Exception e)
        {
            Debug.LogWarning("Save failed: " + e.Message);
        }
    }

    public static bool IsUnlocked(int level) => Data.unlocked.Contains(level);

    public static void CompleteLevel(int level, int score)
    {
        if (score > Data.bestScore) Data.bestScore = score;
        int next = level + 1;
        if (next <= 3 && !Data.unlocked.Contains(next)) Data.unlocked.Add(next);
        Save();
    }

    public static void SetMusicVolume(float v) { Data.musicVol = v; Save(); }
    public static void SetSfxVolume(float v) { Data.sfxVol = v; Save(); }

    public static void ResetProgress()
    {
        _data = new SaveData();
        Save();
    }
}
