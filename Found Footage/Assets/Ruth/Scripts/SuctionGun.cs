
using UnityEngine;
using UnityEngine.InputSystem;

public class SuctionGun : MonoBehaviour
{
    [Header("Weapon Stats")]
    public int damagePerSecond = 5;
    public float range = 10;
    public float magazineSize = 50;
    public float weaponCooldown = 0.5f;

    [Header("Weapon Info")]
    public float ammo = 10;
    public bool isShooting = false;
    public bool canShoot = false;

    [Header("References")]
    public InputActionReference fireAction;
    public Transform attackPoint;
    //public RaycastHit rayHit;
    public string enemyTag;

    [Header("Private")]
    private bool isButtonHeld = false;

    // Unity Functions

    void Start()
    {
        fireAction.action.started += StartFireEvent;
        fireAction.action.canceled += FinishFireEvent;
    }

    void Update()
    {
        if (isShooting)
        {
            // Shoot update

            if (ammo <= 0)
                StopShooting();
            else
            {
                // Raycast

                /*
                if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out rayHit, range, whatIsEnemy))
                {
                    Debug.Log(rayHit.collider.name);

                    //enemy must be tagged as enemy to work and needs to have a script with a take damage function
                    //if (rayHit.collider.CompareTag("Enemy"))
                    //rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
                }*/
            }
        }
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


    // Functions

    public void BeginShooting()
    {
        if (!canShoot || isShooting || ammo <= 0) return;

        isShooting = true;
        Debug.Log("Begin Shooting");
    }

    void StopShooting()
    {
        Debug.Log("Stop Shooting");
        Invoke("ResetCD", weaponCooldown);
    }

    void ResetCD() 
    { 
        canShoot = true;

        if (isButtonHeld)
            BeginShooting();
    }
}
