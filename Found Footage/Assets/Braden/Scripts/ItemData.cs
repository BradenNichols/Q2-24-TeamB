using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [Header("Settings")]
    public Vector3 holdPositionOffset;
    public Vector3 holdRotationOffset;

    [Header("Read-Only")]
    public bool isHeld = false;
    [HideInInspector]
    public Backpack heldBackpack;
}