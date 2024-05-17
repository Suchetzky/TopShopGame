using UnityEngine;

public class SceneReloader : MonoBehaviour
{
    private void Update()
    {
        // Reload the current scene when the "R" key is pressed
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("GameTutorial");
        }

        // Close the program when the "Esc" key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    private void ReloadScene()
    {
        int currentSceneIndex =  UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(currentSceneIndex);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}