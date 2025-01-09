using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInteraction : MonoBehaviour
{
    public GameObject idk;
    public Backpack backpack;

    public void interact ()
    {
        GameObject getgun = Instantiate(idk);
        backpack.AddItem(getgun);
        backpack.EquipItem(getgun);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
