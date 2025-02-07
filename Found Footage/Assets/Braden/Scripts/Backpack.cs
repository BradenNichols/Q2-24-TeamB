using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Backpack : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    public Camera topRenderCamera;
    public InputActionReference gunEquipAction;
    public InputActionReference flashlightEquipAction;

    [Header("Data")]
    public bool canEquip = true;

    [Header("Runtime")]
    [HideInInspector]
    public GameObject heldItem;
    ItemData heldItemData;

    [Header("Settings")]
    public Vector3 itemPositionOffset = Vector3.zero;
    public float itemPositionLerpSpeed;
    public Vector3 itemRotationOffset = Vector3.zero;
    public float itemRotationLerpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // Input

        gunEquipAction.action.started += EquipGunEvent;
        flashlightEquipAction.action.started += EquipFlashlightEvent;

        // ITEM SETUP

        GameObject gunItem = GameObject.Find("Protogeist");
        GameObject flashItem = GameObject.Find("Flashlight");

        if (gunItem)
            AddItem(gunItem);

        if (flashItem)
            AddItem(flashItem);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (heldItem != null)
        {
            // Lerp Item Position & Rotation

            Vector3 newPosition = getHeldPosition();
            Quaternion newRotation = getHeldRotation();

            heldItem.transform.rotation = Quaternion.Slerp(heldItem.transform.rotation, newRotation, Time.deltaTime * itemRotationLerpSpeed);
            heldItem.transform.position = Vector3.Lerp(heldItem.transform.position, newPosition, Time.deltaTime * itemPositionLerpSpeed);
        }
    }

    // Held Stuff

    Vector3 getHeldPosition()
    {
        Vector3 itemPosOffset = Vector3.zero;

        if (heldItemData)
            itemPosOffset = heldItemData.holdPositionOffset;

        Vector3 newPosition = cameraTransform.position + ( // item position offset
            cameraTransform.forward * itemPosOffset.z +
            cameraTransform.right * itemPosOffset.x +
            cameraTransform.up * itemPosOffset.y
        );

        newPosition += ( // global position offset
            cameraTransform.forward * itemPositionOffset.z +
            cameraTransform.right * itemPositionOffset.x +
            cameraTransform.up * itemPositionOffset.y
        );

        return newPosition;
    }

    Quaternion getHeldRotation()
    {
        Vector3 itemRotOffset = Vector3.zero;

        if (heldItemData)
            itemRotOffset = heldItemData.holdRotationOffset;

        Quaternion newRotation = cameraTransform.rotation 
            * Quaternion.Euler(itemRotOffset.x, itemRotOffset.y, itemRotOffset.z); // item rotation offset

        newRotation *= Quaternion.Euler(itemRotationOffset.x, itemRotationOffset.y, itemRotationOffset.z); // global rotation offset

        return newRotation;
    }

    // Input

    public void EquipGunEvent(InputAction.CallbackContext context)
    {
        GameObject gun = FindItem("Protogeist");

        if (gun != null && Time.timeScale > 0)
            EquipItem(gun);
    }

    public void EquipFlashlightEvent(InputAction.CallbackContext context)
    {
        GameObject flashlight = FindItem("Flashlight");

        if (flashlight != null && Time.timeScale > 0)
            EquipItem(flashlight);
    }

    // Public Functions

    public List<GameObject> GetItems()
    {
        List<GameObject> items = new();

        for (int i = 0; i < transform.childCount; i++)
            items.Add(transform.GetChild(i).gameObject);

        return items;
    }

    public GameObject FindItem(string ItemName)
    {
        foreach (GameObject thisItem in GetItems())
        {
            if (thisItem.name.Contains(ItemName))
                return thisItem;
        }

        return null;
    }

    public void AddItem(GameObject item)
    {
        item.transform.parent = transform; // parent it to backpack
        item.layer = LayerMask.NameToLayer("TopRender");
        item.SetActive(false);

        Collider collider = item.GetComponent<Collider>();

        if (collider)
            collider.enabled = false;
    }

    public void EquipItem(GameObject item, bool forceEquip = false)
    {
        if (heldItem == item || item == null || (!forceEquip && !canEquip)) return;
        else if (heldItem != null) UnequipItem();

        heldItem = item;

        heldItemData = item.GetComponent<ItemData>();
        heldItemData.isHeld = true;
        heldItemData.heldBackpack = this;

        topRenderCamera.fieldOfView = heldItemData.holdFieldOfView;

        item.transform.rotation = getHeldRotation();
        item.transform.position = getHeldPosition();

        item.SetActive(true);

        BaseItem itemClass = item.GetComponent<BaseItem>();

        if (itemClass)
        {
            itemClass.Equip();

            if (!canEquip)
            {
                itemClass.isDisabled = true;
                itemClass.Disable();
            }
        }   
    }

    public void UnequipItem()
    {
        if (heldItem != null)
        {
            BaseItem itemClass = heldItem.GetComponent<BaseItem>();

            if (itemClass)
            {
                if (itemClass.isDisabled)
                {
                    itemClass.isDisabled = false;
                    itemClass.Enable();
                }

                itemClass.Unequip();
            }

            heldItem.SetActive(false);
            heldItemData.isHeld = false;
            heldItemData.heldBackpack = null;

            topRenderCamera.fieldOfView = 60;
        }

        heldItem = null;
    }
}
