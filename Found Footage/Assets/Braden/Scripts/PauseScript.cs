using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [Header("State")]
    public bool canPause = true;
    public bool isPaused = false;

    [Header("References")]
    public GameObject pauseMenu;
    public InputActionReference pauseAction;

    /*
    [Header("Pixel References")]
    public RawImage pixelsImage;
    public Camera playerCamera;
    public RenderTexture lowPixels;
    public RenderTexture medPixels;*/

    // Input
    void Start()
    {
        pauseAction.action.started += PauseInputEvent;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseInputEvent(InputAction.CallbackContext context)
    {
        SetPaused(!isPaused);
    }

    // Main
    public void SetPaused(bool paused = true)
    {
        if (paused == isPaused) return;

        if (paused)
        {
            if (!canPause) return;

            pauseMenu.SetActive(true);

            //playerCamera.targetTexture = medPixels;
            //pixelsImage.texture = medPixels;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);

            //playerCamera.targetTexture = lowPixels;
            //pixelsImage.texture = lowPixels;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            Time.timeScale = 1;
        }

        isPaused = paused;
    }

    // Buttons
    public void Quit()
    {
        Application.Quit();
    }
    public void Continue()
    {
        SetPaused(false);
    }
    public void MainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("TitleScreen");
    }
}
