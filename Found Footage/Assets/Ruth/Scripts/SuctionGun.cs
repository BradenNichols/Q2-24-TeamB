
using System;
using Unity.VisualScripting;
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

    [Header("Recoil")]
    public float itemRecoilDivide = 1500f;
    public float itemRecoilClamp = 0.15f;
    public Vector2 cameraRecoil;

    [Header("Weapon Info")]
    public float ammo = 10;
    public bool canShoot = true;
    bool shootCooldown = false;
    [HideInInspector]
    public bool isShooting = false;

    [Header("References")]
    public InputActionReference fireAction;
    public InputActionReference lookAction;
    public Tooltip noBatteryTip;

    public ParticleSystem gasParticle;
    ParticleSystemRenderer gasRenderer;

    public ParticleSystem beamParticle;
    ParticleSystemRenderer beamRenderer;

    public Material hitGas;
    Material defaultGas;

    public Material hitBeam;
    Material defaultBeam;

    public AudioSource activeSound;
    public string enemyTag;

    [Header("Private")]
    bool isButtonHeld = false;
    float activeSoundVolume;
    ItemData itemData;
    ViewCount viewCount;
    MainCamera cameraScript;

    Vector3 baseHoldPosition;

    // OLD POS OFFSET: Vector3(1, -1.2, 1.2)

    // Unity Functions

    void Start()
    {
        fireAction.action.started += StartFireEvent;
        fireAction.action.canceled += FinishFireEvent;

        itemData = GetComponent<ItemData>();
        noBatteryTip = GetComponent<Tooltip>();
        activeSoundVolume = activeSound.volume;

        beamRenderer = beamParticle.GetComponent<ParticleSystemRenderer>();
        defaultBeam = beamRenderer.material;

        gasRenderer = gasParticle.GetComponent<ParticleSystemRenderer>();
        defaultGas = gasRenderer.material;

        cameraScript = GameObject.Find("PlayerCam").GetComponent<MainCamera>();
        viewCount = GameObject.Find("Player").GetComponent<ViewCount>();

        GameObject batteryTip = GameObject.Find("NoBatteryTooltip");

        if (batteryTip)
            noBatteryTip = batteryTip.GetComponent<Tooltip>();

        baseHoldPosition = itemData.holdPositionOffset;
    }
    void Update()
    {
        if (isShooting)
        {
            // Recoil

            float recoilX = (UnityEngine.Random.Range(-100f, 100f) / 100f) * cameraRecoil.x;
            float recoilY = (UnityEngine.Random.Range(-100f, 100f) / 100f) * cameraRecoil.y;

            Vector2 recoil = new Vector2(recoilX * Time.deltaTime, recoilY * Time.deltaTime);
            cameraScript.AddRotation(recoil);

            // Item Recoil

            float newItemRecoilX = Mathf.Clamp(itemData.holdPositionOffset.x + (recoilX / itemRecoilDivide), baseHoldPosition.x - itemRecoilClamp, baseHoldPosition.x + itemRecoilClamp);
            float newItemRecoilY = Mathf.Clamp(itemData.holdPositionOffset.y + (recoilY / itemRecoilDivide), baseHoldPosition.y - itemRecoilClamp, baseHoldPosition.y + itemRecoilClamp);

            itemData.holdPositionOffset = new Vector3(newItemRecoilX, newItemRecoilY, baseHoldPosition.z);
        }
    }

    void FixedUpdate()
    {
        if (isShooting) // Shoot update
        {
            if (ammo <= 0) {
                if (noBatteryTip && noBatteryTip.playedTimes == 0)
                    noBatteryTip.Play();

                StopShooting();
                return;
            }

            ammo = Mathf.Clamp(ammo - (Time.fixedDeltaTime * ammoDepletionPerSecond), 0, ammo);

            // Raycast

            bool hitEnemy = false;

            Transform firePoint = cameraScript.transform;
            Vector3 firePosition = firePoint.position + (firePoint.right * 0.4f);

            RaycastHit hitInfo;

            if (Physics.BoxCast(firePosition, (firePoint.localScale / 2) * castSizeMult, firePoint.forward, out hitInfo, firePoint.rotation, castRange)) // returns a bool if hit
            {
                GameObject hitObject = hitInfo.collider.gameObject;
                //Debug.Log(hitObject.name);

                if (hitObject.CompareTag("Enemy"))
                {
                    // do stuff
                    GeneralStats enemyStats = hitObject.GetComponent<GeneralStats>();

                    if (!enemyStats)
                        enemyStats = hitObject.GetComponentInParent<GeneralStats>();

                    if (enemyStats && enemyStats.isAlive)
                    {
                        enemyStats.TakeDamage(damagePerSecond * Time.fixedDeltaTime);
                        hitEnemy = true;

                        if (!enemyStats.isAlive) // we killed them
                        {
                            viewCount.AddViewers(UnityEngine.Random.Range(enemyStats.minViewerAdd, enemyStats.maxViewerAdd));
                        }
                    }
                }
            }

            // Update Hitting

            if (hitEnemy)
            { 
                beamRenderer.material = hitBeam;
                gasRenderer.material = hitGas;

                cameraScript.sensitivityMultiplier = hitSensitivity;
                viewCount.AddViewers(0); // reset the decay
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
        if (Time.timeScale == 0) return;

        isButtonHeld = true;
        BeginShooting();
    }

    public void FinishFireEvent(InputAction.CallbackContext context)
    {
        isButtonHeld = false;
        StopShooting();
    }

    // Backpack

    public override void Disable()
    {
        StopShooting();
    }

    public override void Enable()
    {
        ResetCD(); // will start shooting if held
    }

    public override void Equip()
    {
        shootCooldown = true;
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
        if (!canShoot || isShooting || shootCooldown || ammo <= 0 || !itemData.isHeld || isDisabled) return;

        isShooting = true;

        gasParticle.Play();
        beamParticle.Play();

        activeSound.volume = activeSoundVolume;
        activeSound.Play();
    }

    void StopShooting(bool ResetCD = true)
    {
        if (!isShooting) return;

        isShooting = false;
        shootCooldown = true;

        gasParticle.Stop();
        beamParticle.Stop();

        itemData.holdPositionOffset = baseHoldPosition;

        activeSound.volume = 0; // prevent popping sound
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
