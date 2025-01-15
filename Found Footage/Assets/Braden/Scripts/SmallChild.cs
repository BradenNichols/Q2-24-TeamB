using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallChild : BaseEnemy
{
    [Header("SmallMare AI")]
    public string aiState = "Patrol";
    public float distanceToChase = 20;
    [HideInInspector] public List<Transform> patrolPoints = new();

    [SerializeField] Transform patrolGoal;

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
                if (isSlowed || Vector3.Distance(transform.position, playerTransform.position) <= distanceToChase)
                    aiState = "Chase";
            }
            else if (aiState == "Chase")
            {
                target = playerTransform;
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
