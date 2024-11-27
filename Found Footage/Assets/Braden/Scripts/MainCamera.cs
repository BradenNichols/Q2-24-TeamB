using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float sensitivity;
    public Transform orientation;

    private float xRotation;
    private float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // mouse
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * (sensitivity * 400);
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * (sensitivity * 400);

        yRotation += mouseX;
        xRotation = Mathf.Clamp(xRotation - mouseY, -90f, 90f);

        // rotate
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
