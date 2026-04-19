using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public static string lastScene = "Backstage";
    public void GoBack()
    {
        Change(lastScene);
    }
    public void Change(string newScene)
    {
        SceneManager.LoadScene(newScene);
        lastScene = newScene;
    }
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    public void QuitOut()
    {
        Application.Quit();
    }
}
