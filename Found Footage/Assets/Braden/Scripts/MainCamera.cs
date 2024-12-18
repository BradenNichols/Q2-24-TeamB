using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{
    public float mouseSensitivity;
    public float controllerSensitivity;
    public float cameraLerpSpeed;
    bool isController = false;

    public Transform orientation;

    float xRotation;
    float yRotation;

    Vector2 lookAxis = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        //QualitySettings.vSyncCount = 0;  // VSync must be disabled
        //Application.targetFrameRate = 20;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    
    void Update()
    {
        float sensitivity = mouseSensitivity;

        if (isController)
            sensitivity = controllerSensitivity;

        // axis
        float axisX = lookAxis.x * Time.deltaTime * (sensitivity * 6);
        float axisY = lookAxis.y * Time.deltaTime * (sensitivity * 6);

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
