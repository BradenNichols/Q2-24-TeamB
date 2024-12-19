using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [Header("Item Holding Data")]
    public Vector3 holdPositionOffset;
    public Vector3 holdRotationOffset;
    public float holdFieldOfView = 60;

    [Header("Read-Only")]
    public bool isHeld = false;
    [HideInInspector]
    public Backpack heldBackpack;
}