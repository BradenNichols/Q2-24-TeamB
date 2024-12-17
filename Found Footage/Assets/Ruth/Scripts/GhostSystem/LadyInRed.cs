using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadyInRed : GhostStats
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // the LadyInRed touched a Player
        }
    }
}
