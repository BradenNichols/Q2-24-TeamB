using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CreditsScreen : MonoBehaviour
{
    public InputActionReference escapeInput;
    public InputActionReference nextSlideInput;
    public InputActionReference lastSlideInput;
    public List<Image> slides;

    Image pastSlide;
    int slideIndex;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        escapeInput.action.started += EscapeEvent;
        nextSlideInput.action.started += NextSlideEvent;
        lastSlideInput.action.started += LastSlideEvent;

        slideIndex = Random.Range(0, slides.Count - 1);
        NextSlide(0);
    }

    // Input
    public void NextSlideEvent(InputAction.CallbackContext context) { NextSlide(1); }
    public void LastSlideEvent(InputAction.CallbackContext context) { NextSlide(-1); }
    public void EscapeEvent(InputAction.CallbackContext context) { Exit(); }

    // Functions
    public void Exit()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    void NextSlide(int indexChange)
    {
        slideIndex += indexChange;

        if (slideIndex < 0)
            slideIndex = slides.Count - 1;
        else if (slideIndex >= slides.Count)
            slideIndex = 0;

        if (pastSlide)
            pastSlide.enabled = false;

        Image currentSlide = slides[slideIndex];
        currentSlide.enabled = true;

        pastSlide = currentSlide;
    }
}
