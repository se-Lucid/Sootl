using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public static string lastScene = "ClassRooms";
    public string sceneName;
    public static void GoBack()
    {
        Change(lastScene);
    }
    public static void Change(string load)//to hard load a scene like after a gameover or post cutscene
    {
        if(load != null)// && load != lastScene
        {
            SceneManager.LoadScene(load);
        }
        //SceneManager.UnloadSceneAsync("GameOver");

        //lastScene = load;
    }
    public static void SoftTransition(string toLoad = null, string toUnload = null)
    {
        if (toLoad != null)
        {
            SceneManager.LoadSceneAsync(toLoad);
        }
        if (toUnload != null)
        {
            SceneManager.UnloadSceneAsync(toUnload);
        }
    }
    public static void GameOver()
    {
        SceneManager.LoadScene("GameOver");
        Cursor.lockState = CursorLockMode.None;
    }
    public static void QuitOut()
    {
        Application.Quit();
    }
    private void OnTriggerEnter(Collider other)
    {
        Change(sceneName);
    }
}
