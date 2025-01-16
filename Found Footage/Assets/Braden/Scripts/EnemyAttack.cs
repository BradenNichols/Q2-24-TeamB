using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Animator animator;
    public BaseEnemy enemyClass;

    bool hasAttacked = false;

    void OnTriggerEnter(Collider other)
    {
        if (!enemyClass.isTouchActive || !enemyClass.stats || !enemyClass.stats.isAlive) return;

        if (other.gameObject.CompareTag("Player") && !hasAttacked)
        {
            // the Enemy touched a Player

            GameObject player = other.gameObject;
            hasAttacked = true;

            GeneralStats generalStats = player.GetComponentInParent<GeneralStats>();
            generalStats.Kill();

            // TODO: make the player face the enemy

            StartCoroutine(Kill());
        }
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(1f);
        Debug.Log("FIN");
    }
}
