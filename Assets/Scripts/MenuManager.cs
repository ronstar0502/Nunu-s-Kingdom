using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void NewGame() // change to 1 for intro/tutorial
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Continue(GameObject pauseMenu)
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        print($"current scene index: {sceneIndex}");
        SceneManager.LoadScene(sceneIndex);
    }
}
