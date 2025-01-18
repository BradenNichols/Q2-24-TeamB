using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintTip : MonoBehaviour
{
    bool hasTriggered = false;
    Tooltip tip;

    void Start()
    {
        tip = GetComponent<Tooltip>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.gameObject.CompareTag("Player"))
        {
            hasTriggered = true;
            tip.Play();
        }
    }
}
