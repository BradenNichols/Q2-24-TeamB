using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : BaseItem
{
    [Header("Item Stats")]
    public float maxBattery = 100;
    public float batteryDrainPerSecond = 30;
    public float shakeBatteryGain = 15;
    public float shakeTime = 0.5f;
    public Vector3 shakePosition;
    public Vector3 shakeRotation;

    [Header("Item Info")]
    public float battery = 50;
    public bool isEnabled = false;
    public bool canShake = true;
    [HideInInspector]
    public bool isShaking = false;

    [Header("References")]
    public Light spotLight;
    public InputActionReference fireAction;

    [Header("Private")]
    private ItemData itemData;
    private Vector3 baseRotation;
    private Vector3 basePosition;
    private float baseIntensity;

    // Unity Functions

    void Awake()
    {
        itemData = GetComponent<ItemData>();
        baseRotation = itemData.holdRotationOffset;
        basePosition = itemData.holdPositionOffset;

        fireAction.action.started += StartFireEvent;
        baseIntensity = spotLight.intensity;
    }

    void Update()
    {
        // Values

        if (isShaking)
        {
            itemData.holdPositionOffset = shakePosition;
            itemData.holdRotationOffset = shakeRotation;
        } else
        {
            itemData.holdPositionOffset = basePosition;
            itemData.holdRotationOffset = baseRotation;

            if (itemData.isHeld)
            {
                battery = Mathf.Clamp(battery - (batteryDrainPerSecond * Time.deltaTime), 0, maxBattery);

                if (battery <= 0)
                    EnableFlashlight(false);
            }
        }

        // Light Intensity

        float batteryThreshold = maxBattery / 1.5f;

        if (battery <= batteryThreshold)
        {
            float ratio = battery / batteryThreshold;
            spotLight.intensity = ratio * baseIntensity;
        }
        else
            spotLight.intensity = baseIntensity;
    }

    // Input

    public void StartFireEvent(InputAction.CallbackContext context)
    {
        ShakeFlashlight();
    }

    // Backpack

    public override void Equip()
    {
        ShakeFlashlight();
    }

    public override void Unequip()
    {
        itemData.holdPositionOffset = basePosition;
        itemData.holdRotationOffset = baseRotation;
    }

    // Main

    void EnableFlashlight(bool enabled)
    {
        if (isEnabled == enabled) return;

        isEnabled = enabled;
        spotLight.enabled = enabled;
    }

    // Shake

    public void ShakeFlashlight()
    {
        if (!canShake || isShaking || !itemData.isHeld) return;

        isShaking = true;
        EnableFlashlight(false);

        battery = Mathf.Clamp(battery + shakeBatteryGain, 0, maxBattery);

        Invoke("StopShake", shakeTime);
    }

    void StopShake()
    {
        if (!isShaking) return;

        isShaking = false;
        spotLight.enabled = true;

        EnableFlashlight(true);
    }
}
