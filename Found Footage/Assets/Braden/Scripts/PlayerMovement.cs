using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool canJump = true;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundMask;
    bool isGrounded;

    [Header("References")]
    public Transform orientation;

    Vector2 moveInput;
    Vector3 moveDirection;
    Rigidbody body;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;

        Physics.gravity = new Vector3(0, -16f, 0); // default is -9.81
    }

    // Set data on update
    void Update()
    {
        // Speed Control
        Vector3 flatVelocity = new Vector3(body.velocity.x, 0, body.velocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            body.velocity = new Vector3(limitedVelocity.x, body.velocity.y, limitedVelocity.z);
        }

        // Drag
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        if (isGrounded)
            body.drag = groundDrag;
        else
            body.drag = 0;
    }

    // Move player on fixed update
    void FixedUpdate()
    {
        moveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x; // works with ur rotation

        Vector3 moveForce = moveDirection.normalized * moveSpeed * 10f;

        if (!isGrounded)
            moveForce *= airMultiplier;

        body.AddForce(moveForce, ForceMode.Force);
    }

    // [Input Events] //

    public void MoveEvent(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void JumpEvent(InputAction.CallbackContext context)
    {
        bool isPressed = context.ReadValue<float>() > 0;

        if (!isGrounded || !canJump || !isPressed)
            return;

        canJump = false;

        body.velocity = new Vector3(body.velocity.x, 0, body.velocity.z); // reset Y velocity
        body.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        Invoke(nameof(ResetJump), jumpCooldown);
    }
    void ResetJump() { canJump = true; }
}
