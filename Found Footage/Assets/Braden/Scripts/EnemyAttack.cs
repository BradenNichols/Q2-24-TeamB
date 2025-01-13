using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public BaseEnemy enemyClass;

    void OnCollisionEnter(Collision collision)
    {
        if (!enemyClass.isTouchActive || !enemyClass.stats || !enemyClass.stats.isAlive) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            // the Enemy touched a Player

            GeneralStats generalStats = collision.gameObject.GetComponent<GeneralStats>();
            generalStats.Kill();
        }
    }
}
