using UnityEngine;

public class BoardLayout : MonoBehaviour
{
    public LayoutRow[] allRows;  // Mảng chứa tất cả các hàng

    public Gem[,] GetLayout()
    {
        // Tạo mảng 2D: width = số gem trong hàng đầu tiên, height = số hàng
        Gem[,] theLayout = new Gem[allRows[0].gemsInRow.Length, allRows.Length];

        // Duyệt qua tất cả các hàng
        for (int y = 0; y < allRows.Length; y++)
        {
            // Duyệt qua từng gem trong hàng
            for (int x = 0; x < allRows[y].gemsInRow.Length; x++)
            {
                // Kiểm tra x còn nằm trong phạm vi width
                if (x < theLayout.GetLength(0))
                {
                    // Nếu vị trí này có gem
                    if (allRows[y].gemsInRow[x] != null)
                    {
                        // Lưu gem vào mảng 2D, ĐẢO NGƯỢC chiều Y
                        theLayout[x, allRows.Length - 1 - y] = allRows[y].gemsInRow[x];
                    }
                }
            }
        }

        return theLayout;
    }
}

// Đây là một lớp để lưu trữ thông tin về bố cục của bảng, bao gồm các hàng và các viên ngọc trong mỗi hàng
[System.Serializable]
public class LayoutRow
{
    public Gem[] gemsInRow;
}
