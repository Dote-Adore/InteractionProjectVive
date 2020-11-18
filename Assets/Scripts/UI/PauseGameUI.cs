using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngineInternal;

public class PauseGameUI : MonoBehaviour
{
    public CameraRotation cameraRotate;

    public Interact interact;

    public PlayerMove playerMove;
    // Start is called before the first frame update
    void Start()
    {
        if (interact == null)
        {
            interact = FindObjectOfType<Interact>();
        }

        if (cameraRotate == null)
        {
            cameraRotate = FindObjectOfType<CameraRotation>();
        }

        if (playerMove == null)
        {
            playerMove = FindObjectOfType<PlayerMove>();
        }
    }
    public void continueGame()
    {
        if (cameraRotate != null)
        {
            cameraRotate.lockCursor = true;
            cameraRotate.lockRotation = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            cameraRotate.lockRotation = false;
            this.gameObject.SetActive(false);
            if (interact != null)
            {
                interact.disableInteract = false;
            }

            if (playerMove != null)
            {
                playerMove.canMove = true;
            }
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
