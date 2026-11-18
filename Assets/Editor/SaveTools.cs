#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

// Лежит в Assets/Editor. Добавляет пункты меню Tools -> Save для проверки сохранения.
public static class SaveTools
{
    static string Dir => Application.persistentDataPath;
    static string FilePath => Path.Combine(Dir, "save.json");

    [MenuItem("Tools/Save/Open Save Folder")]
    static void OpenFolder()
    {
        Directory.CreateDirectory(Dir);
        EditorUtility.RevealInFinder(File.Exists(FilePath) ? FilePath : Dir + "/");
        Debug.Log("Save folder: " + Dir);
    }

    [MenuItem("Tools/Save/Print Save File")]
    static void PrintFile()
    {
        if (File.Exists(FilePath)) Debug.Log(FilePath + "\n" + File.ReadAllText(FilePath));
        else Debug.Log("No save.json yet. Folder: " + Dir);
    }

    [MenuItem("Tools/Save/Break Save File (test)")]
    static void BreakFile()
    {
        Directory.CreateDirectory(Dir);
        File.WriteAllText(FilePath, "broken{{{");
        Debug.Log("save.json is now broken on purpose");
    }

    [MenuItem("Tools/Save/Delete Save File")]
    static void DeleteFile()
    {
        if (File.Exists(FilePath)) File.Delete(FilePath);
        Debug.Log("save.json deleted");
    }
}
#endif
