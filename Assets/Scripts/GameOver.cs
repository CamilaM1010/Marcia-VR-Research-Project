using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    // Called when "Restart" button is clicked
    public void RestartGame()
    {
        SceneManager.LoadScene("Classroom");
    }

    // Called when "Quit to Menu" button is clicked
    public void QuitToMenu()
    {
        // Replace "MainMenu" with the actual name of your menu scene
        SceneManager.LoadScene("Opening");
    }

    // Optional: Exit game if it's a build
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
