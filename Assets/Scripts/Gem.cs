using UnityEngine;

public class Gem : MonoBehaviour
{
    // Vị trí của gem trong mảng 2D
    public Vector2Int posIndex;
    // Tham chiếu đến board chứa gem
    public Board board;

    // Vị trí touch đầu và cuối
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;

    // Trạng thái nhấn chuột
    private bool mousePressed;
    // Góc vuốt (degrees): 0°=phải, 90°=lên, 180°=trái, -90°=xuống
    private float swipeAngle = 0;
    // Gem sẽ hoán đổi vị trí
    private Gem otherGem;

    private void Update()
    {
        // Di chuyển gem về vị trí mục tiêu nếu chưa đến
        if (Vector2.Distance(transform.position, posIndex) > .01f)
        {
            transform.position = Vector2.Lerp(transform.position, posIndex, board.gemSpeed * Time.deltaTime);
        }
        else
        {
            // Đảm bảo gem ở đúng vị trí và cập nhật mảng board
            transform.position = new Vector3(posIndex.x, posIndex.y, 0f);
            board.allGems[posIndex.x, posIndex.y] = this;
        }

        // Phát hiện khi thả chuột
        if (mousePressed && Input.GetMouseButtonUp(0))
        {
            mousePressed = false;
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }

    // Khởi tạo gem với vị trí và board
    public void SetupGem(Vector2Int pos, Board theBoard)
    {
        posIndex = pos;
        board = theBoard;
    }

    // Phát hiện khi nhấn chuột vào gem
    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePressed = true;
    }

    // Tính góc vuốt từ vị trí đầu đến cuối
    private void CalculateAngle()
    {
        // Tính góc bằng Atan2 và chuyển từ radian sang degrees
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x);
        swipeAngle = swipeAngle * 180 / Mathf.PI;
        Debug.Log($"Swipe Angle: {swipeAngle} degrees");

        // Chỉ di chuyển nếu vuốt đủ xa (>0.5 unit)
        if (Vector3.Distance(firstTouchPosition, finalTouchPosition) > 0.5f)
        {
            MovePieces();
        }
    }

    // Di chuyển gem theo hướng vuốt
    private void MovePieces()
    {
        // Vuốt phải: góc -45° đến 45°
        if (swipeAngle < 45 && swipeAngle > -45 && posIndex.x < board.width - 1)
        {
            otherGem = board.allGems[posIndex.x + 1, posIndex.y];
            otherGem.posIndex.x--;
            posIndex.x++;
        }
        // Vuốt lên: góc 45° đến 135°
        else if (swipeAngle > 45 && swipeAngle <= 135 && posIndex.y < board.height - 1)
        {
            otherGem = board.allGems[posIndex.x, posIndex.y + 1];
            otherGem.posIndex.y--;
            posIndex.y++;
        }
        // Vuốt xuống: góc -135° đến -45°
        else if (swipeAngle < -45 && swipeAngle >= -135 && posIndex.y > 0)
        {
            otherGem = board.allGems[posIndex.x, posIndex.y - 1];
            otherGem.posIndex.y++;
            posIndex.y--;
        }
        // Vuốt trái: góc >135° hoặc <-135°
        else if ((swipeAngle > 135 || swipeAngle <= -135) && posIndex.x > 0)
        {
            otherGem = board.allGems[posIndex.x - 1, posIndex.y];
            otherGem.posIndex.x++;
            posIndex.x--;
        }

        // Cập nhật vị trí mới trong mảng board
        board.allGems[posIndex.x, posIndex.y] = this;
        if (otherGem != null)
            board.allGems[otherGem.posIndex.x, otherGem.posIndex.y] = otherGem;
    }
}
