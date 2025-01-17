using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAttack : MonoBehaviour
{
    public Animator animator;
    public BaseEnemy enemyClass;
    public float deathLookVertical;

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

            // info

            Vector3 enemyPos = enemyClass.transform.position;

            MainCamera playerCamera = GameObject.Find("PlayerCam").GetComponent<MainCamera>();
            Vector3 playerPos = playerCamera.GetComponentInParent<Transform>().position;

            // make the enemy face the player
            enemyClass.transform.LookAt(playerPos);

            // make the player face the enemy
            // https://discussions.unity.com/t/difference-between-quaternion-and-lookat/734449/2

            Quaternion lookAt = Quaternion.LookRotation(enemyPos - playerPos);
            Vector2 vector = new Vector2(deathLookVertical, lookAt.eulerAngles.y);

            playerCamera.canLook = false;
            playerCamera.RotateTo(vector);

            // start kill process
            StartCoroutine(Kill());
        }
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(0.3f);
        animator.SetBool("IsAttacking", true);
        yield return new WaitForSeconds(0.85f);

        Fade fadeOut = GameObject.Find("FadeIn").GetComponent<Fade>();
        fadeOut.fadeTime = 0.2f;
        fadeOut.endAlpha = 1;
        fadeOut.enabled = true;

        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("TitleScreen");
    }
}
