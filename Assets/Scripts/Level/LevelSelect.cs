using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelect : MonoBehaviour
{
    private void Start()
    {
        // Đảm bảo LevelData tồn tại
        if (LevelData.Instance == null)
        {
            // Nếu vào trực tiếp scene LevelSelect (debug), tạo LevelData
            GameObject levelDataObj = new GameObject("LevelData");
            levelDataObj.AddComponent<LevelData>();
        }
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

}
