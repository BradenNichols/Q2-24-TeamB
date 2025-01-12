using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{
    [Header("Sensitivity")]
    public Vector2 mouseSensitivity;
    public Vector2 controllerSensitivity;
    public Vector2 sensitivityMultiplier = Vector2.one;

    [Header("Values")]
    public float cameraLerpSpeed;
    public float FOVLerpSpeed;
    [HideInInspector]
    public float targetFOV;
    [HideInInspector]
    public float baseFOV;

    bool isController = false;

    [Header("References")]
    public Transform orientation;
    Camera playerCamera;

    float xRotation;
    float yRotation;

    Vector2 lookAxis = Vector2.zero;

    void Start()
    {
        playerCamera = GetComponent<Camera>();

        baseFOV = playerCamera.fieldOfView;
        targetFOV = baseFOV;
    }

    // Update is called once per frame

    void Update()
    {
        // Field of View

        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, targetFOV, Time.deltaTime * FOVLerpSpeed);

        // Camera

        if (!transform || !orientation)
            return;

        Vector2 sensitivity = mouseSensitivity;

        if (isController)
            sensitivity = controllerSensitivity;

        sensitivity *= sensitivityMultiplier;

        // Axis
        float axisX = lookAxis.x * Time.deltaTime * (sensitivity.x * 6);
        float axisY = lookAxis.y * Time.deltaTime * (sensitivity.y * 6);

        yRotation += axisX;
        xRotation = Mathf.Clamp(xRotation - axisY, -90f, 90f);

        // Rotate
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
