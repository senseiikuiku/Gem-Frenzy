using UnityEngine;

public class Gem : MonoBehaviour
{
    //[HideInInspector]
    public Vector2Int posIndex;
    //[HideInInspector]
    public Board board;

    private void Start()
    {

    }

    public void SetupGem(Vector2Int pos, Board theBoard)
    {
        posIndex = pos; // Lưu trữ vị trí của viên ngọc trên bảng
        board = theBoard; // Lưu trữ tham chiếu đến bảng

    }
}
