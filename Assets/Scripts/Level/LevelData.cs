using UnityEngine;

public class LevelData : MonoBehaviour
{
    public static LevelData Instance { get; private set; }

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

    // Lưu số sao đạt được cho level
    public void SaveLevelStars(int levelNumber, int stars)
    {
        string key = "Level_" + levelNumber + "_Stars";

        // Chỉ lưu nếu số sao mới cao hơn số sao cũ
        int currentStars = PlayerPrefs.GetInt(key, 0);
        if (stars > currentStars)
        {
            PlayerPrefs.SetInt(key, stars);
            PlayerPrefs.Save();
        }

        if (stars > 0)
        {
            UnlockLevel(levelNumber + 1);
        }
    }

    public int GetLevelStars(int levelNumber)
    {
        string key = "Level_" + levelNumber + "_Stars";
        return PlayerPrefs.GetInt(key, 0);
    }

    // Mở khóa level
    private void UnlockLevel(int levelNumber)
    {
        string key = "Level_" + levelNumber + "_Unlocked";
        PlayerPrefs.SetInt(key, 1);
        PlayerPrefs.Save();
    }

    // Kiểm tra level đã mở khóa chưa
    public bool IsLevelUnlocked(int levelNumber)
    {
        if (levelNumber == 1) return true; // Level 1 luôn được mở khóa

        string key = "Level_" + levelNumber + "_Unlocked";
        return PlayerPrefs.GetInt(key, 0) == 1; // 
    }

    // Lưu level hiện tại đang chơi
    public void SetCurrentLevel(int levelNumber)
    {
        PlayerPrefs.SetInt("CurrentLevel", levelNumber);
        PlayerPrefs.Save();
    }

    // Lấy level hiện tại
    public int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt("CurrentLevel", 1);
    }

    // Reset toàn bộ progress (dùng cho debug)
    public void ResetAllProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

}
