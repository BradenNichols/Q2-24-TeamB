using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralStats : MonoBehaviour
{
    [Header("Health")]
    public float health = 1;
    public float maxHealth = 1;

    [Header("Info")]
    public bool isAlive = true;
    public string characterType = "";

    [Header("On Death")]
    public int minViewerAdd = 0;
    public int maxViewerAdd = 0;

    public void TakeDamage(float damage)
    {
        if (!isAlive) return;

        health = Mathf.Clamp(health - damage, 0, maxHealth);

        if (health <= 0)
            Kill();
    }

    public void Kill()
    {
        if (!isAlive) return;

        isAlive = false;
        health = 0;

        Destroy(gameObject); // temporary
    }
}