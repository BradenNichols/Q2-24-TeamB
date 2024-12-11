using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGhost : MonoBehaviour
{
    public float speed;
    public float damage;
    public int resistance;
    public GameObject target;

    public BaseGhost(int attack, int defense)
    {
        /*
        this.speed = speed;
        this.damage = damage;
        this.resistance = resistance;
        this.target = target;*/
    }
}