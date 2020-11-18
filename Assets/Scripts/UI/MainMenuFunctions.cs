using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuFunctions : MonoBehaviour
{
    public Canvas StartGameTipsCanvas;
    // 主界面开始游戏 
    public void OnClickStartBtn()
    {
        if (StartGameTipsCanvas != null)
        {
            StartGameTipsCanvas.gameObject.SetActive(true);
            Invoke("OpenLevel",2);
        }
        else
        {
            OpenLevel();
        }
        Debug.Log("res");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
#endif
     Application.Quit();   
    }

    private void OpenLevel()
    {
        SceneManager.LoadScene("Scene/OperatingRoom");

    }
    
}
