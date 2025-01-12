using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSet : MonoBehaviour
{
    public Transform cameraPosition;

    // Update is called once per frame
    void Update()
    {
        if (!transform || !cameraPosition) return;
        transform.position = cameraPosition.position;
    }
}
