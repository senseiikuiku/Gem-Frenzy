using UnityEngine;

public class Board : MonoBehaviour
{
    public int width; // Số lượng ô theo chiều ngang
    public int height; // Số lượng ô theo chiều dọc

    public GameObject bgTilePrefab; // Prefab của ô nền

    public Gem[] gems; // Mảng chứa các viên ngọc
    public Gem[,] allGems; // Mảng 2D để lưu trữ các viên ngọc trên bảng

    private void Start()
    {
        allGems = new Gem[width, height]; // Khởi tạo mảng 2D để lưu trữ các viên ngọc
        Setup(); // Gọi hàm Setup để tạo các ô nền khi bắt đầu trò chơi
    }

    private void Setup()
    {
        // Tạo các ô nền dựa trên kích thước đã định
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2 pos = new Vector2(x, y);
                GameObject bgTile = Instantiate(bgTilePrefab, pos, Quaternion.identity);
                bgTile.transform.parent = this.transform; // Đặt bgTile làm con của Board để dễ quản lý
                bgTile.name = $"BGTile_{x},{y}"; // Đặt tên cho ô nền

                // Tạo ngẫu nhiên một viên ngọc và đặt nó vào vị trí tương ứng
                int gemToUse = Random.Range(0, gems.Length);
                SpawnGem(new Vector2Int(x, y), gems[gemToUse]);
            }
        }
    }

    private void SpawnGem(Vector2Int pos, Gem gemToSpawn)
    {
        // Tạo một viên ngọc mới tại vị trí đã cho và đặt nó làm con của Board
        Gem gem = Instantiate(gemToSpawn, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        gem.transform.parent = this.transform;
        gem.name = $"Gem_{pos.x},{pos.y}";
        allGems[pos.x, pos.y] = gem; // Lưu trữ viên ngọc vào mảng 2D

        gem.SetupGem(pos, this); // Đặt vị trí và tham chiếu đến Board trong viên ngọc
    }
}
