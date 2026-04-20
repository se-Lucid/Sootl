using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public static string lastScene = "Backstage";
    public static void GoBack()
    {
        Change(lastScene);
    }
    public static void Change(string newScene)
    {
        SceneManager.LoadScene(newScene);
        lastScene = newScene;
        Cursor.lockState = CursorLockMode.Locked;
    }
    public static void GameOver()
    {
        SceneManager.LoadScene("GameOver");
        Cursor.lockState = CursorLockMode.None;
    }
    public static void QuitOut()
    {
        Cursor.lockState = CursorLockMode.None;
        Application.Quit();
    }
}
