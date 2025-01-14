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
    public float sprintBobAdd = 5;
    public float sprintSoundPitch = 1.2f;
    public Vector3 sprintPositionOffset;
    public Vector3 sprintRotationOffset;
    public bool canSprint = true;
    public bool isSprinting = false;

    [Header("Head Bob")]
    public float defaultBobbingSpeed = 14;
    public float bobbingAmount = 0.05f;
    float defaultBobPosY = 0;
    float bobTimer = 0;

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
    public AudioSource footstepsSound;

    [Header("Private")]
    public Vector2 moveInput;
    public Vector3 moveDirection;
    Rigidbody body;

    float moveSpeed;

    void Start()
    {
        body = GetComponent<Rigidbody>();
        body.freezeRotation = true;

        defaultBobPosY = camera.transform.localPosition.y;
        Physics.gravity = new Vector3(0, -16f, 0); // default is -9.81
    }

    // Set data on update
    void Update()
    {
        // Check sprinting movement
        if (moveInput.y <= 0 && isSprinting)
            setSprinting(false);

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

        // Headbob
        float bobSpeed = defaultBobbingSpeed;

        if (isSprinting)
            bobSpeed += sprintBobAdd;

        if (moveDirection.magnitude > 0)
        {
            bobTimer += Time.deltaTime * bobSpeed;
            camera.transform.localPosition = new Vector3(camera.transform.localPosition.x, 
                defaultBobPosY + Mathf.Sin(bobTimer) * bobbingAmount, camera.transform.localPosition.z);
        } else
        {
            bobTimer = 0;
            camera.transform.localPosition = 
                new Vector3(camera.transform.localPosition.x, 
                Mathf.Lerp(camera.transform.localPosition.y, defaultBobPosY, Time.deltaTime * bobSpeed), camera.transform.localPosition.z);
        }

        // Drag
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);

        if (isGrounded)
            body.drag = groundDrag;
        else
            body.drag = 0;

        // Footsteps
        if (moveDirection.magnitude > 0 && isGrounded)
        {
            if (isSprinting)
                footstepsSound.pitch = sprintSoundPitch;
            else
                footstepsSound.pitch = 1;

            footstepsSound.enabled = true;
        }
        else
        {
            footstepsSound.enabled = false;
        }
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

        if (state == true && moveInput.y <= 0)
            return;

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
