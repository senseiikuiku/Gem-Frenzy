using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text scoreText;
    public TMP_Text targetText;

    public TMP_Text winScore;
    public TMP_Text winText;
    public GameObject winStartParent;
    public GameObject winStartPrefab;
    public int winStartCount = 0;

    public GameObject pausePanel;
    public GameObject roundOverGreen;

    [Header("Options Button")]
    public GameObject[] btns;

    private void Start()
    {
        pausePanel.SetActive(false);
        roundOverGreen.SetActive(false);

        // Xóa các star prefab đã tồn tại trong winStartParent
        foreach (Transform child in winStartParent.transform)
        {
            if (child.gameObject.CompareTag("Star"))
            {
                Destroy(child.gameObject);
            }
        }

    }

    public void SpawnStar(int starCount)
    {
        for (int i = 0; i < starCount; i++)
        {
            GameObject star = Instantiate(winStartPrefab, winStartParent.transform);
            star.transform.localPosition = Vector3.zero; // Đặt vị trí của star về (0, 0, 0) trong winStartParent
        }
    }

    public void TogglePausePanel()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);

        if (pausePanel.activeSelf)
        {
            Time.timeScale = 0f; // Tạm dừng game
        }
        else
        {
            Time.timeScale = 1f; // Tiếp tục game
        }
    }

    public void ChangeScene(string sceneName)
    {
        Time.timeScale = 1f; // Đảm bảo game không bị tạm dừng khi chuyển scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
