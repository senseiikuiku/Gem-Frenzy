using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string nameScene;

    public void LevelSelect()
    {
        SceneManager.LoadScene(nameScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
