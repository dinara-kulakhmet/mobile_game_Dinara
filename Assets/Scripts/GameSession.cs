using UnityEngine;

public static class GameSession
{
    public static LevelData Current;
    public static int Score;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    static void ResetStatics()
    {
        Current = null;
        Score = 0;
    }
}