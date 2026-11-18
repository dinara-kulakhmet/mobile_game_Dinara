using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Level Data", fileName = "Level_01")]
public class LevelData : ScriptableObject
{
    public int levelNumber = 1;
    public string displayName = "Level 1";
    public float timeLimit = 90f;
    public GameObject layoutPrefab;   
    public Vector2 playerSpawn;
    public Vector2 goalPosition;
    public List<SpawnEntry> enemies = new List<SpawnEntry>();
    public List<SpawnEntry> items = new List<SpawnEntry>();
}

[Serializable]
public struct SpawnEntry
{
    public GameObject prefab;
    public Vector2 position;
}