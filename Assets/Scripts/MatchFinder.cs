using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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
    }


}
