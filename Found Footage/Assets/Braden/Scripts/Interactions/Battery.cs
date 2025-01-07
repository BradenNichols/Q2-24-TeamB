using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Battery : MonoBehaviour
{
    [Header("Stats")]
    public float batteryRefillAmount = 100;

    [Header("References")]
    private Backpack playerBackpack;

    void Start()
    {
        GameObject backpackObject = GameObject.Find("Backpack");
        playerBackpack = backpackObject.GetComponent<Backpack>();
    }

    public void DoInteract()
    {
        GameObject ghostGun = playerBackpack.FindItem("Protogeist");

        if (ghostGun != null)
        {
            SuctionGun gunScript = ghostGun.GetComponent<SuctionGun>();
            gunScript.AddAmmo(batteryRefillAmount);
        }

        Destroy(gameObject);
    }
}
