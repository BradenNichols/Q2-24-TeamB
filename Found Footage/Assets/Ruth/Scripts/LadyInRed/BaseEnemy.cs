using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // the Enemy touched a Player

            GeneralStats generalStats = collision.gameObject.GetComponent<GeneralStats>();
            generalStats.Kill();
        }
    }
}
