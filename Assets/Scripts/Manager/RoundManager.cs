using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoundManager : MonoBehaviour
{
    private Board board;
    private UIManager uiMan;

    public float roundTime = 60f;
    private bool endingRound = false;

    public int currentScore = 0;
    public int scoreTarget = 5000;
    public float displayScore;
    public float scoreSpeed = 2f;

    private int starsEarned = 0;
    private int currentLevel;

    private void Awake()
    {
        uiMan = FindAnyObjectByType<UIManager>();
        board = FindAnyObjectByType<Board>();

        // Lấy level hiện tại đang chơi
        if (LevelData.Instance != null)
        {
            currentLevel = LevelData.Instance.GetCurrentLevel();
        }
    }

    private void Start()
    {
        if (GameManager.Instance != null)
        {
            roundTime = GameManager.Instance.GetValueRoundTimeLimit();
            scoreTarget = GameManager.Instance.GetValueScoreTarget();
        }

        uiMan.targetText.text = scoreTarget.ToString("0");
    }

    void Update()
    {
        if (roundTime > 0)
        {
            roundTime -= Time.deltaTime;

            if (roundTime <= 0)
            {
                roundTime = 0;

                endingRound = true;
            }
        }

        if (endingRound && board.currentState == Board.BoardState.move)
        {
            WinCheck();
            endingRound = false;
        }

        uiMan.timeText.text = roundTime.ToString("0.0") + "s";

        displayScore = Mathf.Lerp(displayScore, currentScore, Time.deltaTime * scoreSpeed);
        uiMan.scoreText.text = displayScore.ToString("0");
    }

    private void WinCheck()
    {
        uiMan.roundOverGreen.SetActive(true);
        uiMan.winScore.text = currentScore.ToString("0");

        // Tính toán số sao dựa trên tỷ lệ điểm đạt được
        float scoreRatio = (float)currentScore / scoreTarget;
        switch (scoreRatio)
        {
            case float n when (n >= 1f):
                uiMan.winText.text = "Perfect!";
                starsEarned = 5;
                break;
            case float n when (n >= 0.8f):
                uiMan.winText.text = "Excellent!";
                starsEarned = 4;
                break;
            case float n when (n >= 0.6f):
                uiMan.winText.text = "Good!";
                starsEarned = 3;
                break;
            case float n when (n >= 0.4f):
                uiMan.winText.text = "Not Bad!";
                starsEarned = 2;
                break;
            case float n when (n >= 0.2f):
                uiMan.winText.text = "Needs Improvement!";
                starsEarned = 1;
                break;
            default:
                uiMan.winText.text = "Try Again!";
                starsEarned = 0;
                break;
        }

        for (int i = 0; i < uiMan.btns.Length; i++)
        {
            if (starsEarned > 0 && i == 0)
            {
                uiMan.btns[i].SetActive(true); // Hiển thị nút Next Level nếu có sao
            }
            else
            {
                uiMan.btns[i].SetActive(false); // Ẩn nút Restart Level nếu không có sao
            }
            uiMan.btns[uiMan.btns.Length - 1].SetActive(true); // Luôn hiển thị nút Back to Menu
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLevelCompleteSound();
        }

        uiMan.winStartCount = starsEarned;
        uiMan.SpawnStar(uiMan.winStartCount);

        // Lưu kết quả và mở khóa level tiếp theo
        SaveLevelResult();
    }

    private void SaveLevelResult()
    {
        if (LevelData.Instance != null)
        {
            LevelData.Instance.SaveLevelStars(currentLevel, starsEarned);
        }
    }

    // Các hàm UI để gọi từ button
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        if (LevelData.Instance != null)
        {
            int nextLevel = LevelData.Instance.GetCurrentLevel() + 1;

            // Kiểm tra xem có level tiếp theo không
            if (nextLevel <= GetTotalLevels())
            {
                LevelData.Instance.SetCurrentLevel(nextLevel);
                SceneManager.LoadScene("Level");
            }
            else
            {
                // Hết level, quay về menu chọn level
                BackToLevelSelect();
            }
        }
    }

    public void BackToLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private int GetTotalLevels()
    {
        if (LevelManager.Instance != null)
        {
            return LevelManager.Instance.levelButtons.Length;
        }
        else
        {
            return 0; // Trả về 0 nếu không tìm thấy LevelManager
        }
    }
}
