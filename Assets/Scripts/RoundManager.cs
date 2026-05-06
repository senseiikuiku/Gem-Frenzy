using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private Board board;
    private UIManager uiMan;

    public float roundTime = 60f;

    private bool endingRound = false;

    public int currentScore = 0;
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
    }
}
