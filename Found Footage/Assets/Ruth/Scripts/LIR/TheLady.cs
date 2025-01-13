using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheLady : MonoBehaviour
{
    //Dragon Script in video(EP 03)
    public int HP = 100;
    public Animator animator;

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if(HP <= 0)
        {
            //Play Death Animation
            animator.SetTrigger("die");
        }
        else
        {
            //Play Get Hit Animation
            animator.SetTrigger("damage");
        }
    }
}
