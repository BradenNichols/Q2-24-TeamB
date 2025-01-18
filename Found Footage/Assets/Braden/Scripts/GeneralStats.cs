using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GeneralStats : MonoBehaviour
{
    [Header("Health")]
    public float health = 1;
    public float maxHealth = 1;
    public bool isImmune = false;

    [Header("Info")]
    public bool isAlive = true;
    public string characterType = "";
    [HideInInspector] public float? lastDamaged;

    [Header("On Death")]
    public int minViewerAdd = 0;
    public int maxViewerAdd = 0;
    public Animator animator;
    public AudioSource mareFinalDeathSound;
    public UnityEvent deathEvent;

    void Update()
    {
        if (lastDamaged != null)
            lastDamaged += Time.deltaTime;
    }

    public void TakeDamage(float damage)
    {
        if (!isAlive || isImmune) return;

        health = Mathf.Clamp(health - damage, 0, maxHealth);
        lastDamaged = 0;

        if (health <= 0)
            Kill();
    }

    public void Kill()
    {
        if (!isAlive) return;

        isAlive = false;
        health = 0;

        deathEvent.Invoke();

        if (characterType == "SmallChild")
        {
            animator.SetBool("IsDead", true);
            Destroy(gameObject, 6f); // temporary
        } else if (characterType == "TheMare")
        {
            // this is mostly handled in their AI code
            TheMare mare = GetComponent<TheMare>();
            GeneralStats playerStats = GameObject.Find("Player").GetComponent<GeneralStats>();

            if (mare && mare.isFinalSpawn && playerStats.isAlive)
            {
                // State

                playerStats.Kill(); // stop them from moving and other AI from moving but otherwise does nothing
                animator.SetBool("IsDead", true);

                if (mareFinalDeathSound)
                    mareFinalDeathSound.Play();

                // misc

                GameObject musicObject = GameObject.Find("Music");

                if (musicObject)
                {
                    AudioSource music = musicObject.GetComponent<AudioSource>();
                    music.Stop();
                }

                StartCoroutine(WinGame());
            }
        }
    }

    // Fin

    IEnumerator WinGame()
    {
        yield return new WaitForSeconds(3f);

        Fade fadeOut = GameObject.Find("FadeIn").GetComponent<Fade>();
        fadeOut.fadeTime = 1.7f;
        fadeOut.endAlpha = 1;

        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("Credits");
    }
}