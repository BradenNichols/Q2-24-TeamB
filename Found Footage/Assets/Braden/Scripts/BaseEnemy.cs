using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    [Header("Enemy Data")]
    public bool isTouchActive;
    public bool shouldSlowOnShot = true;
    public float slowTimeOnShot = 0.6f;
    public float slowOnShotSpeed = 2.5f;

    [Header("AI")]
    public Transform target;
    public bool shouldPathToTarget = true;

    [Header("References")]
    public GeneralStats stats;
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected Animator animator;
    protected Transform playerTransform;
    protected float baseSpeed;

    protected void Start()
    {
        baseSpeed = agent.speed;
        playerTransform = GameObject.Find("Player").transform;
    }

    protected void Update()
    {
        if (shouldPathToTarget && target)
        {
            agent.SetDestination(target.position);
        }

        if (shouldSlowOnShot)
        {
            if (stats.lastDamaged != null && stats.lastDamaged <= slowTimeOnShot)
                agent.speed = slowOnShotSpeed;
            else
                agent.speed = baseSpeed;
        }

        animator.SetBool("IsMoving", agent.velocity.magnitude > 0);
    }
}
