using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class MatchFinder : MonoBehaviour
{
    private Board board;

    public List<Gem> currentMatches = new List<Gem>();

    private void Awake()
    {
        board = FindAnyObjectByType<Board>();
    }

    public void FindAllMatches()
    {
        // Trước khi tìm kiếm các cặp khớp, hãy xóa danh sách currentMatches để đảm bảo rằng nó chỉ chứa các cặp khớp hiện tại.
        currentMatches.Clear();

        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                Gem currentGem = board.allGems[x, y];
                if (currentGem != null)
                {
                    // Kiểm tra theo chiều ngang
                    if (x > 0 && x < board.width - 1)
                    {
                        Gem leftGem = board.allGems[x - 1, y];
                        Gem rightGem = board.allGems[x + 1, y];
                        if (leftGem != null && rightGem != null)
                        {
                            if (leftGem.type == currentGem.type && rightGem.type == currentGem.type)
                            {
                                leftGem.isMatched = true;
                                rightGem.isMatched = true;
                                currentGem.isMatched = true;

                                currentMatches.Add(leftGem);
                                currentMatches.Add(rightGem);
                                currentMatches.Add(currentGem);
                            }
                        }
                    }

                    // Kiểm tra theo chiều dọc
                    if (y > 0 && y < board.height - 1)
                    {
                        Gem upGem = board.allGems[x, y + 1];
                        Gem downGem = board.allGems[x, y - 1];
                        if (upGem != null && downGem != null)
                        {
                            if (upGem.type == currentGem.type && downGem.type == currentGem.type)
                            {
                                upGem.isMatched = true;
                                downGem.isMatched = true;
                                currentGem.isMatched = true;

                                currentMatches.Add(upGem);
                                currentMatches.Add(downGem);
                                currentMatches.Add(currentGem);
                            }
                        }
                    }
                }

            }
        }

        // Loại bỏ các phần tử trùng lặp trong danh sách currentMatches
        if (currentMatches.Count > 0)
        {
            currentMatches = currentMatches.Distinct().ToList();
        }

        CheckForBombs();
    }

    // Hàm kiểm tra xem có bất kỳ viên ngọc nào trong currentMatches là bom hay không
    public void CheckForBombs()
    {
        for (int i = 0; i < currentMatches.Count; i++)
        {
            Gem gem = currentMatches[i];

            int x = gem.posIndex.x;
            int y = gem.posIndex.y;

            // Kiểm tra các viên ngọc xung quanh viên ngọc hiện tại để xem có bom nào không
            if (gem.posIndex.x > 0)
            {
                if (board.allGems[x - 1, y] != null)
                {
                    if (board.allGems[x - 1, y].type == Gem.GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x - 1, y), board.allGems[x - 1, y]);
                    }
                }
            }

            // Kiểm tra viên ngọc bên phải
            if (gem.posIndex.x < board.width - 1)
            {
                if (board.allGems[x + 1, y] != null)
                {
                    if (board.allGems[x + 1, y].type == Gem.GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x + 1, y), board.allGems[x + 1, y]);
                    }
                }
            }

            // Kiểm tra viên ngọc bên dưới
            if (gem.posIndex.y > 0)
            {
                if (board.allGems[x, y - 1] != null)
                {
                    if (board.allGems[x, y - 1].type == Gem.GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x, y - 1), board.allGems[x, y - 1]);
                    }
                }
            }

            // Kiểm tra viên ngọc bên trên
            if (gem.posIndex.y < board.height - 1)
            {
                if (board.allGems[x, y + 1] != null)
                {
                    if (board.allGems[x, y + 1].type == Gem.GemType.bomb)
                    {
                        MarkBombArea(new Vector2Int(x, y + 1), board.allGems[x, y + 1]);
                    }
                }
            }
        }
    }

    // Hàm đánh dấu khu vực ảnh hưởng của bom
    public void MarkBombArea(Vector2Int bombPos, Gem theBomb)
    {
        // Duyệt qua tất cả các viên ngọc trong khu vực ảnh hưởng của bom và đánh dấu chúng là đã khớp
        for (int x = bombPos.x - theBomb.blastSize; x <= bombPos.x + theBomb.blastSize; x++)
        {
            for (int y = bombPos.y - theBomb.blastSize; y <= bombPos.y + theBomb.blastSize; y++)
            {
                if (x >= 0 && x < board.width && y >= 0 && y < board.height)
                {
                    if (board.allGems[x, y] != null)
                    {
                        board.allGems[x, y].isMatched = true;
                        currentMatches.Add(board.allGems[x, y]);

                    }

                }
            }
        }
        currentMatches = currentMatches.Distinct().ToList(); // Loại bỏ các phần tử trùng lặp trong danh sách currentMatches sau khi thêm các viên ngọc từ khu vực bom
    }
}
