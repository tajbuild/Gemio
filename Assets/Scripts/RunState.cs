using UnityEngine;

public static class RunState
{
    public static int Score { get; private set; }
    public static int ScoreAtLevelStart { get; private set; }

    // These upgrades remain unlocked while the current run continues.
    public static bool HasDoubleJumpUnlocked { get; private set; }
    public static bool HasEnergyWeaponUnlocked { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetState()
    {
        Score = 0;
        ScoreAtLevelStart = 0;
        HasDoubleJumpUnlocked = false;
        HasEnergyWeaponUnlocked = false;
    }

    public static void StartNewRun()
    {
        Score = 0;
        ScoreAtLevelStart = 0;

        // A new run begins without either upgrade.
        HasDoubleJumpUnlocked = false;
        HasEnergyWeaponUnlocked = false;
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

    public static void UnlockDoubleJump()
    {
        HasDoubleJumpUnlocked = true;
    }

    public static void UnlockEnergyWeapon()
    {
        HasEnergyWeaponUnlocked = true;
    }
}