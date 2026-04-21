using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public static string lastScene = "ClassRooms";
    public string sceneName;
    public static void GoBack()
    {
        Change(lastScene);
        SceneManager.UnloadSceneAsync("GameOver");
    }
    public static void Change(string load)
    {
        if(load != null ||  load != lastScene)
        {
            SceneManager.LoadScene(load);
        }
        //lastScene = load;
    }
    public static void GameOver()
    {
        SceneManager.LoadScene("GameOver");
        Cursor.lockState = CursorLockMode.Confined;
    }
    public static void QuitOut()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Application.Quit();
    }
    private void OnTriggerEnter(Collider other)
    {
        Change(sceneName);
    }
}
