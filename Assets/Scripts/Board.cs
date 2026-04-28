using UnityEngine;

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

    private void Start()
    {
        // Khởi tạo mảng 2D với kích thước width x height
        allGems = new Gem[width, height];
        // Tạo bảng game
        Setup();
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
                SpawnGem(new Vector2Int(x, y), gems[gemToUse]);
            }
        }
    }

    // Tạo gem tại vị trí chỉ định
    private void SpawnGem(Vector2Int pos, Gem gemToSpawn)
    {
        // Instantiate gem tại vị trí (x, y)
        Gem gem = Instantiate(gemToSpawn, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        gem.transform.parent = this.transform;
        gem.name = $"Gem_{pos.x},{pos.y}";

        // Lưu vào mảng 2D
        allGems[pos.x, pos.y] = gem;

        // Setup thông tin vị trí và board cho gem
        gem.SetupGem(pos, this);
    }
}
