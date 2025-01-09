
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SuctionGun : BaseItem
{
    [Header("Weapon Stats")]
    public float damagePerSecond;
    public float castRange = 15;
    public float castSizeMult = 3;
    public float magazineSize = 25;
    public float weaponCooldown = 0.5f;
    public float ammoDepletionPerSecond = 10;
    public Vector2 hitSensitivity;

    [Header("Weapon Info")]
    public float ammo = 10;
    public bool canShoot = true;
    bool shootCooldown = false;
    [HideInInspector]
    public bool isShooting = false;

    [Header("References")]
    public InputActionReference fireAction;
    public InputActionReference lookAction;

    public ParticleSystem gasParticle;
    ParticleSystemRenderer gasRenderer;

    public ParticleSystem beamParticle;
    ParticleSystemRenderer beamRenderer;

    public Material hitGas;
    Material defaultGas;

    public Material hitBeam;
    Material defaultBeam;

    public string enemyTag;

    [Header("Private")]
    private bool isButtonHeld = false;
    private ItemData itemData;
    private MainCamera cameraScript;

    // OLD POS OFFSET: Vector3(1, -1.2, 1.2)

    // Unity Functions

    void Start()
    {
        fireAction.action.started += StartFireEvent;
        fireAction.action.canceled += FinishFireEvent;

        itemData = GetComponent<ItemData>();

        beamRenderer = beamParticle.GetComponent<ParticleSystemRenderer>();
        defaultBeam = beamRenderer.material;

        gasRenderer = gasParticle.GetComponent<ParticleSystemRenderer>();
        defaultGas = gasRenderer.material;

        cameraScript = GameObject.Find("PlayerCam").GetComponent<MainCamera>();
    }

    void Update()
    {
        if (isShooting) // Shoot update
        {
            if (ammo <= 0) { StopShooting(); return; }
            ammo = Mathf.Clamp(ammo - (Time.deltaTime * ammoDepletionPerSecond), 0, ammo);

            // Raycast

            bool hitEnemy = false;

            Transform firePoint = beamParticle.transform;
            RaycastHit hitInfo;

            if (Physics.BoxCast(firePoint.position, (firePoint.localScale / 2) * castSizeMult, firePoint.forward, out hitInfo, firePoint.rotation, castRange)) // returns a bool if hit
            {
                GameObject hitObject = hitInfo.collider.gameObject;
                //Debug.Log(hitObject.name);

                if (hitObject.CompareTag("Enemy"))
                {
                    // do stuff
                    GeneralStats enemyStats = hitObject.GetComponent<GeneralStats>();

                    if (enemyStats && enemyStats.isAlive)
                    {
                        enemyStats.TakeDamage(damagePerSecond * Time.deltaTime);
                        hitEnemy = true;
                    }
                }
            }

            // Update Hitting

            if (hitEnemy)
            { 
                beamRenderer.material = hitBeam;
                gasRenderer.material = hitGas;

                cameraScript.sensitivityMultiplier = hitSensitivity;
            }
            else
            { 
                beamRenderer.material = defaultBeam;
                gasRenderer.material = defaultGas;

                cameraScript.sensitivityMultiplier = Vector2.one;
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

    // Backpack

    public override void Equip()
    {
        Invoke("ResetCD", weaponCooldown);
    }

    public override void Unequip()
    {
        StopShooting(false); // add cooldown and clear stuff
    }

    // Other

    public void AddAmmo(float amount)
    {
        ammo = Mathf.Clamp(ammo + amount, 0, 100);
    }

    // Functions

    public void BeginShooting()
    {
        if (!canShoot || isShooting || shootCooldown || ammo <= 0 || !itemData.isHeld) return;

        isShooting = true;

        gasParticle.Play();
        beamParticle.Play();
    }

    void StopShooting(bool ResetCD = true)
    {
        if (!isShooting) return;

        isShooting = false;
        shootCooldown = true;

        gasParticle.Stop();
        beamParticle.Stop();

        cameraScript.sensitivityMultiplier = Vector2.one;

        if (ResetCD)
            Invoke("ResetCD", weaponCooldown);
    }

    void ResetCD() 
    {
        shootCooldown = false;

        if (isButtonHeld)
            BeginShooting();
    }
}
