using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
 
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }
    
    public void StartGameTutorial()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameTutorial");
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
