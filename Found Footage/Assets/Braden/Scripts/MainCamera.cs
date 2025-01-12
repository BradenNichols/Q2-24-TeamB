using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{
    public Vector2 mouseSensitivity;
    public Vector2 controllerSensitivity;

    public Vector2 sensitivityMultiplier = Vector2.one;

    public float cameraLerpSpeed;
    bool isController = false;

    public Transform orientation;

    float xRotation;
    float yRotation;

    Vector2 lookAxis = Vector2.zero;

    // Update is called once per frame
    
    void Update()
    {
        if (!transform || !orientation)
            return;

        Vector2 sensitivity = mouseSensitivity;

        if (isController)
            sensitivity = controllerSensitivity;

        sensitivity *= sensitivityMultiplier;

        // axis
        float axisX = lookAxis.x * Time.deltaTime * (sensitivity.x * 6);
        float axisY = lookAxis.y * Time.deltaTime * (sensitivity.y * 6);

        yRotation += axisX;
        xRotation = Mathf.Clamp(xRotation - axisY, -90f, 90f);

        // rotate
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(xRotation, yRotation, 0), Time.deltaTime * cameraLerpSpeed);
        orientation.rotation = Quaternion.Slerp(orientation.rotation, Quaternion.Euler(0, yRotation, 0), Time.deltaTime * cameraLerpSpeed);
    }

    public void LookEvent(InputAction.CallbackContext context)
    {
        lookAxis = context.ReadValue<Vector2>();

        if (context.control.device.displayName.Contains("Controller"))
            isController = true;
        else
            isController = false;
    }
}
