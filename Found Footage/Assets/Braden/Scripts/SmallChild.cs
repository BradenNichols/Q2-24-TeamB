using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallChild : BaseEnemy
{
    [Header("SmallMare AI")]
    public string aiState = "Patrol";
    public float distanceToChase = 20;
    public LayerMask seeLayers;
    [HideInInspector] public List<Transform> patrolPoints = new();
    [SerializeField] Transform patrolGoal;

    [Header("Sound")]
    public AudioSource screamSound;
    public AudioSource chaseSound;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        // TODO: add patrol and chase

        if (stats.isAlive && playerTransform)
        {
            if (aiState == "Patrol")
            {
                // patrol points
                if (!patrolGoal || agent.remainingDistance <= agent.stoppingDistance + pathEndThreshold)
                {
                    patrolGoal = patrolPoints[Random.Range(0, patrolPoints.Count)];
                    target = patrolGoal;
                }

                // check for chase

                bool shouldChase = isSlowed;

                if (!shouldChase)
                {
                    bool close = Vector3.Distance(transform.position, playerTransform.position) <= distanceToChase;

                    if (close)
                    {
                        RaycastHit hit;
                        Physics.Raycast(transform.position, playerTransform.position - transform.position, out hit, distanceToChase + 0.5f, seeLayers);

                        if (hit.collider && hit.collider.CompareTag("Player"))
                            shouldChase = true;
                    }
                }

                if (shouldChase)
                {
                    aiState = "Chase";
                    chaseSound.Play();

                    if (screamSound)
                        screamSound.Play();
                }
            }
            else if (aiState == "Chase")
            {
                target = playerTransform;

                if (chaseSound)
                {
                    if (agent.velocity.magnitude > 0)
                        chaseSound.UnPause();
                    else
                        chaseSound.Pause();
                }
            }
        } else
        {
            shouldPathToTarget = false;
            isTouchActive = false;
            shouldSlowOnShot = false;

            agent.speed = 0;
        }

        


        base.Update(); // pathing and such
    }
}
