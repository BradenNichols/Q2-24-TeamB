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
    [SerializeField] protected GeneralStats myStats;
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
            if (myStats.lastDamaged != null && myStats.lastDamaged <= slowTimeOnShot)
                agent.speed = slowOnShotSpeed;
            else
                agent.speed = baseSpeed;
        }

        animator.SetBool("IsMoving", agent.velocity.magnitude > 0);
    }

    // Unity Functions

    void OnCollisionEnter(Collision collision)
    {
        if (!isTouchActive || !myStats || !myStats.isAlive) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            // the Enemy touched a Player

            GeneralStats generalStats = collision.gameObject.GetComponent<GeneralStats>();
            generalStats.Kill();
        }
    }
}
