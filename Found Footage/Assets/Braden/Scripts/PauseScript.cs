using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseScript : MonoBehaviour
{
    [Header("State")]
    public bool canPause = true;
    public bool isPaused = false;

    [Header("References")]
    public GameObject pauseMenu;
    public InputActionReference pauseAction;
    public EventSystem eventSystem;
    public GameObject selectButton;

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
            eventSystem.SetSelectedGameObject(selectButton);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0;
        }
        else
        {
            pauseMenu.SetActive(false);

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
