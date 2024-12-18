using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;

    [Header("Runtime")]
    [HideInInspector]
    public GameObject heldItem;
    ItemData heldItemData;

    [Header("Settings")]
    public float itemPositionLerpSpeed;
    public float itemRotationLerpSpeed;

    // Start is called before the first frame update
    void Start()
    {
        // GUN TESTING
        GameObject testItem = GameObject.Find("Protogeist");
        AddItem(testItem);
        EquipItem(testItem);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (heldItem != null)
        {
            // Lerp Variables

            Vector3 itemPosOffset = Vector3.zero;
            Vector3 itemRotOffset = Vector3.zero;

            if (heldItemData) { 
                itemPosOffset = heldItemData.holdPositionOffset; 
                itemRotOffset = heldItemData.holdRotationOffset; 
            }

            // Lerp Item Position & Rotation

            Vector3 newPosition = cameraTransform.position + (
                cameraTransform.forward * itemPosOffset.z + 
                cameraTransform.right * itemPosOffset.x + 
                cameraTransform.up * itemPosOffset.y
            );

            Quaternion newRotation = cameraTransform.rotation * Quaternion.Euler(itemRotOffset.x, itemRotOffset.y, itemRotOffset.z);

            heldItem.transform.rotation = Quaternion.Slerp(heldItem.transform.rotation, newRotation, Time.deltaTime * itemRotationLerpSpeed);
            heldItem.transform.position = Vector3.Lerp(heldItem.transform.position, newPosition, Time.deltaTime * itemPositionLerpSpeed);
        }
    }

    // Public Functions

    public List<GameObject> GetItems()
    {
        List<GameObject> items = new();

        for (int i = 0; i < transform.childCount; i++)
            items.Add(transform.GetChild(i).gameObject);

        return items;
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

    public void EquipItem(GameObject item)
    {
        if (heldItem == item) return;
        else if (heldItem != null) UnequipItem();

        heldItem = item;

        heldItemData = item.GetComponent<ItemData>();
        heldItemData.isHeld = true;
        heldItemData.heldBackpack = this;

        item.SetActive(true);
    }

    public void UnequipItem()
    {
        if (heldItem != null)
        {
            heldItem.SetActive(false);
            heldItemData.isHeld = false;
            heldItemData.heldBackpack = null;
        }

        heldItem = null;
    }
}
