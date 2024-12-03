using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    [Header("Settings")]
    public Vector3 holdingOffset;

    [Header("Read-Only")]
    public bool isHeld = false;
}