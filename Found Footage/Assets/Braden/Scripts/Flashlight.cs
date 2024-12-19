using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    [Header("Item Stats")]
    public float maxBattery = 100;

    [Header("Item Info")]
    public float battery = 50;
    public bool canShake = true;
    [HideInInspector]
    public bool isShaking = false;

    [Header("References")]
    public InputActionReference fireAction;

    [Header("Private")]
    private ItemData itemData;

    // Unity Functions

    void Start()
    {
        fireAction.action.started += StartFireEvent;

        itemData = GetComponent<ItemData>();
    }

    // Input

    public void StartFireEvent(InputAction.CallbackContext context)
    {
        ShakeFlashlight();
    }

    // Functions

    public void ShakeFlashlight()
    {
        if (!canShake || isShaking) return;

        if (!itemData.isHeld)
        {
            // need to equip
        }

        isShaking = true;
        Debug.Log("SHAKE LIGHT");
    }
}
