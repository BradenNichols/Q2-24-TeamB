using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInteraction : MonoBehaviour
{
    public GameObject idk;
    public Backpack backpack;
    public ViewCount viewCount;
    public Tooltip gunTooltip;
    public GameObject barrier;
    public AudioSource pickupSound;

    public void interact ()
    {
        pickupSound.Play();

        backpack.UnequipItem();

        GameObject getgun = Instantiate(idk);
        backpack.AddItem(getgun);
        backpack.EquipItem(getgun, true);

        viewCount.noDecay = false;
        gunTooltip.Play();

        Destroy(barrier);
        Destroy(gameObject);
    }
}
