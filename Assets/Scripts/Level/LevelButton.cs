using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelNumber;
    public string levelSceneName = "Level"; // Tên scene level chung

    [Header("UI References")]
    public Button button;
    public TMP_Text levelText;
    public GameObject lockIcon;
    public GameObject starParent;
    public GameObject starPrefab;

    public void Start()
    {
        UpdateButtonState();
    }

    // Cập nhật trạng thái button dựa trên dữ liệu level
    private void UpdateButtonState()
    {
        bool isUnlocked = LevelData.Instance.IsLevelUnlocked(levelNumber);

        button.interactable = isUnlocked; // Cập nhật trạng thái button

        // Hiển thị/ẩn icon khóa
        if (lockIcon != null)
        {
            lockIcon.SetActive(!isUnlocked);
        }

        // Hiển thị số level
        if (levelText != null)
        {
            levelText.text = levelNumber.ToString();
        }

        // Hiển thị số sao đã đạt được
        if (isUnlocked)
        {
            int starsEarned = LevelData.Instance.GetLevelStars(levelNumber);
            UpdateStarDisplay(starsEarned);
        }
        else
        {
            UpdateStarDisplay(0);
        }
    }

    private void UpdateStarDisplay(int starCount)
    {
        for (int i = 0; i < starParent.transform.childCount; i++)
        {
            Destroy(starParent.transform.GetChild(i).gameObject);
        }

        for (int i = 0; i < starCount; i++)
        {
            GameObject star = Instantiate(starPrefab, starParent.transform);
            star.transform.localScale = Vector3.one; // Đảm bảo kích thước sao đúng
        }
    }

    public void OnLevelButtonClick()
    {
        // Lưu level hiện tại
        LevelData.Instance.SetCurrentLevel(levelNumber);

        // Load scene level
        SceneManager.LoadScene(levelSceneName);
    }
}
