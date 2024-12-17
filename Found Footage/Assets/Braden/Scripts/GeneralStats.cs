using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralStats : MonoBehaviour
{
    public float health = 1;
    public float maxHealth = 1;
    public bool isAlive = true;

    void TakeDamage(float damage)
    {
        if (!isAlive) return;

        health = Mathf.Clamp(health - damage, 0, maxHealth);

        if (health <= 0)
            Kill();
    }

    void Kill()
    {
        if (!isAlive) return;

        isAlive = false;
        health = 0;
    }
}