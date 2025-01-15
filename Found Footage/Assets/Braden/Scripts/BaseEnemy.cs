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
    public float slowOnShotSpeed = 0.1f;

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

        if (!target)
            target = playerTransform;
    }

    protected void Update()
    {
        if (shouldPathToTarget && target)
        {
            agent.SetDestination(target.position);
        }

        bool isSlowed = shouldSlowOnShot && stats.lastDamaged != null && stats.lastDamaged <= slowTimeOnShot;

        if (isSlowed)
            agent.speed = slowOnShotSpeed;
        else if (agent.speed == slowOnShotSpeed)
            agent.speed = baseSpeed;

        animator.SetBool("IsMoving", agent.velocity.magnitude > 0);
        animator.SetBool("IsBeingDamaged", isSlowed);
    }
}
