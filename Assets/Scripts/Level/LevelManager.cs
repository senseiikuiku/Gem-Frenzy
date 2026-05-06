using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public LevelButton[] levelButtons; // Gán tất cả level buttons vào đây trong Inspector

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

    private void Start()
    {
        // Đảm bảo LevelData được khởi tạo
        if (LevelData.Instance == null)
        {
            GameObject levelDataObj = new GameObject("LevelData");
            levelDataObj.AddComponent<LevelData>();
        }

        // Cập nhật trạng thái tất cả level buttons
        RefreshLevelButtons();
    }

    public void RefreshLevelButtons()
    {
        foreach (LevelButton btn in levelButtons)
        {
            if (btn != null)
            {
                btn.Start(); // Gọi lại Start để cập nhật UI
            }
        }
    }

    // Debug: Reset toàn bộ progress
    public void ResetAllProgress()
    {
        if (LevelData.Instance != null)
        {
            LevelData.Instance.ResetAllProgress();
            RefreshLevelButtons();
        }
    }
}