using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public Fade fadeOut;
    public AudioSource titleMusic;
    public float titleFadeSpeed = 0.8f;

    bool hasPressed = false;
    bool isFadingMusic = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void Update()
    {
        if (isFadingMusic)
            titleMusic.volume -= Time.deltaTime * titleFadeSpeed;
    }

    // Buttons
    public void Quit()
    {
        if (hasPressed) return;
        hasPressed = true;

        Application.Quit();
    }
    public void Play()
    {
        if (hasPressed) return;
        hasPressed = true;
        isFadingMusic = true;

        fadeOut.enabled = true;
        StartCoroutine(PlayYield());
    }
    public void Credits()
    {
        if (hasPressed) return;
        hasPressed = true;

        SceneManager.LoadScene("Credits");
    }

    // Coroutines
    IEnumerator PlayYield()
    {
        yield return new WaitForSeconds(fadeOut.fadeTime);
        SceneManager.LoadScene("ThomasTest");
    }
}
