using System;
using System.Collections.Generic;

// То, что попадает в save.json. JsonUtility не умеет Dictionary, поэтому List.
[Serializable]
public class SaveData
{
    public int version = 1;
    public int bestScore;
    public List<int> unlocked = new List<int> { 1 };
    public float musicVol = 0.8f;
    public float sfxVol = 1f;
}
