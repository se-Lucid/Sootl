using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void OnPlayButton()
    {
        SceneManager.LoadScene("Stage");
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }
}
