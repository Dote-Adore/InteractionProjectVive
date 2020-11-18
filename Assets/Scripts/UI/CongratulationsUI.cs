using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CongratulationsUI : MonoBehaviour
{
    public void turnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }

    public void exitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
#endif
        Application.Quit();   
    }
}
