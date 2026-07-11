using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Game/Level Data")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public string sceneName;
    public bool isUnlocked;
    public int highscore;
}