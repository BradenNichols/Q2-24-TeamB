using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInteraction : MonoBehaviour
{
    [Header("References")]
    public GameObject idk;
    public Backpack backpack;
    public ViewCount viewCount;
    public Tooltip gunTooltip;

    public void interact ()
    {
        GameObject getgun = Instantiate(idk);
        backpack.AddItem(getgun);
        backpack.EquipItem(getgun);

        viewCount.noDecay = false;
        gunTooltip.Type();

        Destroy(gameObject);
    }
}
