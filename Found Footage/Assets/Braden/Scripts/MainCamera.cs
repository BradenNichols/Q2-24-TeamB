using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{
    public float sensitivity;
    public Transform orientation;

    float xRotation;
    float yRotation;

    Vector2 lookAxis = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    
    void Update()
    {
        // axis
        float axisX = lookAxis.x * Time.deltaTime * (sensitivity * 6);
        float axisY = lookAxis.y * Time.deltaTime * (sensitivity * 6);

        yRotation += axisX;
        xRotation = Mathf.Clamp(xRotation - axisY, -90f, 90f);

        // rotate
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void LookEvent(InputAction.CallbackContext context)
    {
        lookAxis = context.ReadValue<Vector2>();

        //print(lookAxis);
    }
}
