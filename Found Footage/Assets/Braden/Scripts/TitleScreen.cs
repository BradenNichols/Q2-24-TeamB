using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
    public Fade fadeOut;
    bool hasPressed = false;

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
