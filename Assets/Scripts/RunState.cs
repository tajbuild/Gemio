using UnityEngine;

public static class RunState
{
    public static int Score { get; private set; }
    public static int ScoreAtLevelStart { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetState()
    {
        Score = 0;
        ScoreAtLevelStart = 0;
    }

    public static void StartNewRun()
    {
        Score = 0;
        ScoreAtLevelStart = 0;
    }

    public static void BeginLevel()
    {
        ScoreAtLevelStart = Score;
    }

    public static void AddScore(int amount)
    {
        Score += amount;
    }

    public static void RestoreLevelStartScore()
    {
        Score = ScoreAtLevelStart;
    }
}