using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuController : MonoBehaviour
{
    public string gameSceneName = "GameScene";
    public GameObject optionsPanel;

    public void StartGame()
    {
        if (string.IsNullOrEmpty(gameSceneName))
        {
            Debug.LogError("Game scene name not set in MainMenuController.");
            return;
        }
        SceneManager.LoadScene(gameSceneName);
    }

    public void OpenOptions()
    {
        // Send player to youtube video as placeholder for options menu
        Application.OpenURL("https://youtu.be/4QBBtebTOdE");
    }

    public void CloseOptions()
    {
        if (optionsPanel != null) optionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
