using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : GeneralStats
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // the LadyInRed touched a Player
            GeneralStats generalStats = collision.gameObject.GetComponent<GeneralStats>();
        }
    }
}
