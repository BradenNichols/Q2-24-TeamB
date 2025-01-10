using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSet : MonoBehaviour
{
    public Transform cameraPosition;
    /*
    public Canvas lowPixelCanvas;

    void Start()
    {
        lowPixelCanvas.enabled = true;
    }*/

    // Update is called once per frame
    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
