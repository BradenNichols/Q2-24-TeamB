using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public float endAlpha = 0;
    public float fadeTime = 0.8f;

    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        image.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        float alpha = Mathf.Lerp(image.color.a, endAlpha, (Time.deltaTime * 5) / fadeTime);
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
    }
}
