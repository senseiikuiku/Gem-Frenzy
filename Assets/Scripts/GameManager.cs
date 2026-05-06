using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetValueWidthHeightBoard()
    {
        int levelNumber = LevelData.Instance.GetCurrentLevel();
        int widthHeightBoard = 0;
        switch (levelNumber)
        {
            case int n when (n <= 5):
                widthHeightBoard = 7;
                break;
            case int n when (n > 5 && n <= 8):
                widthHeightBoard = 8;
                break;
            case int n when (n > 8 && n <= 10):
                widthHeightBoard = 10;
                break;
            default:
                widthHeightBoard = 7;
                break;
        }
        return widthHeightBoard;
    }

    public float GetValueBombChance()
    {
        int levelNumber = LevelData.Instance.GetCurrentLevel();

        int bombChance = 0;

        switch (levelNumber)
        {
            case int n when (n <= 5):
                bombChance = 2;
                break;
            case int n when (n > 5 && n <= 10):
                bombChance = 4;
                break;
            default:
                bombChance = 2;
                break;
        }
        return bombChance;
    }

    public float GetValueRoundTimeLimit()
    {
        int levelNumber = LevelData.Instance.GetCurrentLevel();

        float roundTime = 0f;

        switch (levelNumber)
        {
            case int n when (n <= 5):
                roundTime = 60f;
                break;
            case int n when (n > 5 && n <= 8):
                roundTime = 90;
                break;
            case int n when (n > 8 && n <= 10):
                roundTime = 120;
                break;
            default:
                roundTime = 60f;
                break;
        }

        return roundTime;
    }

    public int GetValueScoreTarget()
    {
        int levelNumber = LevelData.Instance.GetCurrentLevel();
        int scoreTarget = 0;
        switch (levelNumber)
        {
            case int n when (n <= 5):
                scoreTarget = 5000;
                break;
            case int n when (n > 5 && n <= 8):
                scoreTarget = 10000;
                break;
            case int n when (n > 8 && n <= 10):
                scoreTarget = 15000;
                break;
            default:
                scoreTarget = 5000;
                break;
        }
        return scoreTarget;
    }
}
