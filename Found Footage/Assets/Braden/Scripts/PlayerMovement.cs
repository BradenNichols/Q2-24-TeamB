using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float groundDrag;

    public bool isJumpEnabled = false;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool canJump = true;

    [Header("Sprint")]
    public float sprintSpeed = 6.5f;
    public float sprintFOV = 80;
    public Vector3 sprintPositionOffset;
    public Vector3 sprintRotationOffset;
    public bool canSprint = true;
    public bool isSprinting = false;

    [Header("Input")]
    public bool canInteract = true;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundMask;
    bool isGrounded;

    [Header("References")]
    public Transform orientation;
    public Backpack backpack;
    public new MainCamera camera;

    [Header("Private")]
    Vector2 moveInput;
    Vector3 moveDirection;
    Rigidbody body;

    float moveSpeed;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;

        Physics.gravity = new Vector3(0, -16f, 0); // default is -9.81
    }

    // Set data on update
    void Update()
    {
        // Determine Speed
        if (isSprinting)
            moveSpeed = sprintSpeed;
        else
            moveSpeed = walkSpeed;

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

    // [Sprinting] //

    public void setSprinting(bool state)
    {
        if (isSprinting == state || (state == true && !canSprint)) return;

        isSprinting = state;

        if (isSprinting)
        {
            camera.targetFOV = sprintFOV;

            backpack.itemPositionOffset = sprintPositionOffset;
            backpack.itemRotationOffset = sprintRotationOffset;
            backpack.canEquip = false;

            GameObject heldItem = backpack.heldItem;

            if (heldItem != null)
            {
                BaseItem itemClass = heldItem.GetComponent<BaseItem>();

                if (itemClass)
                {
                    itemClass.isDisabled = true;
                    itemClass.Disable();
                }
            }
        } else
        {
            camera.targetFOV = camera.baseFOV;

            backpack.itemPositionOffset = Vector3.zero;
            backpack.itemRotationOffset = Vector3.zero;
            backpack.canEquip = true;

            GameObject heldItem = backpack.heldItem;

            if (heldItem != null)
            {
                BaseItem itemClass = heldItem.GetComponent<BaseItem>();

                if (itemClass)
                {
                    itemClass.isDisabled = false;
                    itemClass.Enable();
                }
            }
        }
    }

    // [Input Events] //

    public void MoveEvent(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void SprintEvent(InputAction.CallbackContext context)
    {
        bool isPressed = context.ReadValue<float>() > 0;
        setSprinting(isPressed);
    }

    public void JumpEvent(InputAction.CallbackContext context)
    {
        bool isPressed = context.ReadValue<float>() > 0;

        if (!isGrounded || !canJump || !isPressed || !isJumpEnabled)
            return;

        canJump = false;

        body.velocity = new Vector3(body.velocity.x, 0, body.velocity.z); // reset Y velocity
        body.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        Invoke(nameof(ResetJump), jumpCooldown);
    }
    void ResetJump() { canJump = true; }
}
