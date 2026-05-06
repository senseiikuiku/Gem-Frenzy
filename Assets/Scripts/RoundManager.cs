using UnityEngine;

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





    private void Awake()
    {
        uiMan = FindAnyObjectByType<UIManager>();
        board = FindAnyObjectByType<Board>();
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
                uiMan.winStartCount = 5;
                break;
            case float n when (n >= 0.8f):
                uiMan.winText.text = "Excellent!";
                uiMan.winStartCount = 4;
                break;
            case float n when (n >= 0.6f):
                uiMan.winText.text = "Good!";
                uiMan.winStartCount = 3;
                break;
            case float n when (n >= 0.4f):
                uiMan.winText.text = "Not Bad!";
                uiMan.winStartCount = 2;
                break;
            case float n when (n >= 0.2f):
                uiMan.winText.text = "Needs Improvement!";
                uiMan.winStartCount = 1;
                break;
            default:
                uiMan.winText.text = "Try Again!";
                uiMan.winStartCount = 0;
                break;

        }

        uiMan.SpawnStar(uiMan.winStartCount);
    }
}
