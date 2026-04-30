using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    // Kích thước bảng (số cột x số hàng)
    public int width;
    public int height;

    // Prefab ô nền
    public GameObject bgTilePrefab;

    // Danh sách các loại gem có thể spawn
    public Gem[] gems;
    // Mảng 2D lưu tất cả gem trên bảng
    public Gem[,] allGems;

    public float gemSpeed;

    [HideInInspector]
    public MatchFinder matchFind;

    public enum BoardState { wait, move }
    public BoardState currentState = BoardState.move;

    private void Awake()
    {
        matchFind = FindAnyObjectByType<MatchFinder>();
    }

    private void Start()
    {
        // Khởi tạo mảng 2D với kích thước width x height
        allGems = new Gem[width, height];

        // Tạo bảng game
        Setup();

    }

    private void Update()
    {
        //matchFind.FindAllMatches();
    }

    // Tạo ô nền và spawn gem cho toàn bộ bảng
    private void Setup()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Tạo ô nền tại vị trí (x, y)
                Vector2 pos = new Vector2(x, y);
                GameObject bgTile = Instantiate(bgTilePrefab, pos, Quaternion.identity);
                bgTile.transform.parent = this.transform;
                bgTile.name = $"BGTile_{x},{y}";

                // Random 1 loại gem và spawn tại vị trí này
                int gemToUse = Random.Range(0, gems.Length);

                int iterations = 0;
                // Kiểm tra nếu spawn gem này sẽ tạo thành 3 gem cùng loại liên tiếp, nếu có thì random lại
                while (MatchesAt(new Vector2Int(x, y), gems[gemToUse]) && iterations < 100)
                {
                    gemToUse = Random.Range(0, gems.Length);
                    iterations++;
                }

                SpawnGem(new Vector2Int(x, y), gems[gemToUse]);
            }
        }
    }

    // Tạo gem tại vị trí chỉ định
    private void SpawnGem(Vector2Int pos, Gem gemToSpawn)
    {
        // Instantiate gem tại vị trí (x, y)
        Gem gem = Instantiate(gemToSpawn, new Vector3(pos.x, pos.y + height, 0f), Quaternion.identity);
        gem.transform.parent = this.transform;
        gem.name = $"Gem_{pos.x},{pos.y}";

        // Lưu vào mảng 2D
        allGems[pos.x, pos.y] = gem;

        // Setup thông tin vị trí và board cho gem
        gem.SetupGem(pos, this);
    }

    // Hàm xử lý kiểm tra nếu spawn gem tại vị trí này sẽ tạo thành 3 gem cùng loại liên tiếp
    bool MatchesAt(Vector2Int posTocheck, Gem gemToCheck)
    {
        // Kiểm tra nếu có 2 gem cùng loại ở bên trái
        if (posTocheck.x > 1)
        {
            if (allGems[posTocheck.x - 1, posTocheck.y].type == gemToCheck.type &&
                allGems[posTocheck.x - 2, posTocheck.y].type == gemToCheck.type)
            {
                return true;
            }
        }

        // Kiểm tra nếu có 2 gem cùng loại ở bên dưới
        if (posTocheck.y > 1)
        {
            if (allGems[posTocheck.x, posTocheck.y - 1].type == gemToCheck.type &&
                allGems[posTocheck.x, posTocheck.y - 2].type == gemToCheck.type)
            {
                return true;
            }
        }
        return false;
    }

    // Hàm xử lý phá hủy gem tại vị trí chỉ định nếu nó đã được đánh dấu là matched
    private void DestroyMatchedGemAt(Vector2Int pos)
    {
        if (allGems[pos.x, pos.y] != null)
        {
            if (allGems[pos.x, pos.y].isMatched)
            {
                Destroy(allGems[pos.x, pos.y].gameObject);
                allGems[pos.x, pos.y] = null;
            }
        }
    }

    // Hàm xử lý phá hủy tất cả gem đã được đánh dấu là matched
    public void DestroyMatches()
    {
        for (int i = 0; i < matchFind.currentMatches.Count; i++)
        {
            if (matchFind.currentMatches[i] != null)
            {
                DestroyMatchedGemAt(matchFind.currentMatches[i].posIndex);
            }
        }

        StartCoroutine(DecreaseRowCo());
    }

    // Coroutine xử lý giảm hàng sau khi phá hủy gem
    private IEnumerator DecreaseRowCo()
    {
        yield return new WaitForSeconds(.2f);

        int nullCounter = 0;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Nếu gặp ô trống thì tăng nullCounter lên
                if (allGems[x, y] == null)
                {
                    nullCounter++;
                }
                // Nếu gặp ô không trống 
                else if (nullCounter > 0)
                {
                    allGems[x, y].posIndex.y -= nullCounter; // Cập nhật vị trí mới của gem sau khi giảm hàng
                    allGems[x, y - nullCounter] = allGems[x, y]; // Di chuyển gem xuống vị trí mới trong mảng
                    allGems[x, y] = null; // Đặt vị trí cũ thành null
                }
            }

            nullCounter = 0;
        }

        StartCoroutine(FillBoardCo());
    }

    // Coroutine xử lý điền lại bảng sau khi giảm hàng
    private IEnumerator FillBoardCo()
    {
        yield return new WaitForSeconds(.5f);
        RefillBoard();

        yield return new WaitForSeconds(.5f);
        matchFind.FindAllMatches();

        if (matchFind.currentMatches.Count > 0)
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
        else
        {
            yield return new WaitForSeconds(.5f);
            currentState = BoardState.move;
        }
    }

    // Hàm xử lý điền lại bảng sau khi giảm hàng
    private void RefillBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allGems[x, y] == null)
                {
                    int gemToUse = Random.Range(0, gems.Length);
                    SpawnGem(new Vector2Int(x, y), gems[gemToUse]);
                }
            }
        }

        CheckMisplaceGems();
    }

    // Hàm xử lý kiểm tra nếu có gem nào bị misplace (không nằm trong mảng 2D) và phá hủy nó
    private void CheckMisplaceGems()
    {
        List<Gem> foundGems = new List<Gem>();

        // Tìm tất cả gem hiện có trong Editor
        foundGems.AddRange(FindObjectsByType<Gem>(FindObjectsSortMode.None));

        // Loại bỏ những gem đã được lưu trong mảng 2D khỏi danh sách foundGems
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (foundGems.Contains(allGems[x, y]))
                {
                    foundGems.Remove(allGems[x, y]);
                }
            }
        }

        foreach (Gem gem in foundGems)
        {
            Destroy(gem.gameObject);
        }
    }
}
