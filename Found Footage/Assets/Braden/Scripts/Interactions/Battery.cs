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
    private AudioSource pickupSound;

    void Start()
    {
        GameObject backpackObject = GameObject.Find("Backpack");

        playerBackpack = backpackObject.GetComponent<Backpack>();
        pickupSound = GetComponent<AudioSource>();
    }

    public void DoInteract()
    {
        GameObject ghostGun = playerBackpack.FindItem("Protogeist");

        if (ghostGun != null)
        {
            SuctionGun gunScript = ghostGun.GetComponent<SuctionGun>();
            gunScript.AddAmmo(batteryRefillAmount);
        }

        // Soft Destroy

        GetComponent<MeshCollider>().enabled = false;
        GetComponent<Light>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;

        // Sound

        if (pickupSound)
            pickupSound.Play();
    }
}
