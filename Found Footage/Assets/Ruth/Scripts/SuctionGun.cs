
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SuctionGun : MonoBehaviour
{
    [Header("Weapon Stats")]
    public float damagePerSecond = 5;
    public float castRange = 15;
    public float castSizeMult = 3;
    public float magazineSize = 25;
    public float weaponCooldown = 0.5f;
    public float ammoDepletionPerSecond = 10;

    [Header("Weapon Info")]
    public float ammo = 10;
    public bool canShoot = true;
    [HideInInspector]
    public bool isShooting = false;

    [Header("References")]
    public InputActionReference fireAction;
    public List<ParticleSystem> fireParticles = new();
    public string enemyTag;

    [Header("Private")]
    private bool isButtonHeld = false;
    private ItemData itemData;

    // OLD POS OFFSET: Vector3(1, -1.2, 1.2)

    // Unity Functions

    void Start()
    {
        fireAction.action.started += StartFireEvent;
        fireAction.action.canceled += FinishFireEvent;

        itemData = GetComponent<ItemData>();
    }

    void Update()
    {
        if (isShooting) // Shoot update
        {
            if (ammo <= 0) { StopShooting(); return; }
            ammo = Mathf.Clamp(ammo - (Time.deltaTime * ammoDepletionPerSecond), 0, ammo);

            // Raycast

            Transform firePoint = itemData.heldBackpack.cameraTransform;
            RaycastHit hitInfo;

            if (Physics.BoxCast(firePoint.position, (firePoint.localScale / 2) * castSizeMult, firePoint.forward, out hitInfo, firePoint.rotation, castRange)) // returns a bool if hit
            {
                GameObject hitObject = hitInfo.collider.gameObject;
                Debug.Log(hitObject.name);

                if (hitObject.CompareTag("Enemy"))
                {
                    // do stuff
                    GeneralStats enemyStats = hitObject.GetComponent<GeneralStats>();

                    if (enemyStats)
                        enemyStats.TakeDamage(damagePerSecond * Time.deltaTime);
                }
            }
        }

        // TODO: show ammo of the gun with a battery percentage
    }

    // Input

    public void StartFireEvent(InputAction.CallbackContext context)
    {
        isButtonHeld = true;
        BeginShooting();
    }

    public void FinishFireEvent(InputAction.CallbackContext context)
    {
        isButtonHeld = false;
        StopShooting();
    }

    // Other

    public void AddAmmo(float amount)
    {
        ammo = Mathf.Clamp(ammo + amount, 0, 100);
    }

    // Functions

    public void BeginShooting()
    {
        if (!canShoot || isShooting || ammo <= 0) return;

        isShooting = true;
        fireParticles.ForEach(particle => particle.Play());

        //Debug.Log("Begin Shooting");
    }

    void StopShooting()
    {
        if (!isShooting) return;

        isShooting = false;
        fireParticles.ForEach(particle => particle.Stop());

        //Debug.Log("Stop Shooting");
        Invoke("ResetCD", weaponCooldown);
    }

    void ResetCD() 
    { 
        canShoot = true;

        if (isButtonHeld)
            BeginShooting();
    }
}
