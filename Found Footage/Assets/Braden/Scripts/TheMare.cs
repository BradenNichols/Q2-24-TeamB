using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheMare : BaseEnemy
{
    [Header("Mare AI")]
    public List<Transform> retreatSpots;
    public bool isFinalSpawn;
    public float retreatWalkSpeed = 8;
    public bool isRetreating = false;
    public AudioSource screamerSound;
    public AudioSource chaseSound;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        screamerSound.Play();

        
    }

    // Update is called once per frame
    new void Update()
    {
        if (!stats.isAlive)
        {
            if (!isFinalSpawn)
            {
                // not final spawn; should retreat

                if (!isRetreating)
                {
                    isRetreating = true;

                    shouldPathToTarget = false;
                    isTouchActive = false;
                    shouldSlowOnShot = false;

                    agent.speed = retreatWalkSpeed;

                    Transform retreat = getFarthestRetreatFromPlayer();
                    agent.SetDestination(retreat.position);
                }
                else // we have set it already
                {
                    if (agent.remainingDistance <= agent.stoppingDistance + pathEndThreshold)
                    { // reached the retreat position
                        Destroy(gameObject);
                        return;
                    }
                }
            } else
            {
                // final spawn, death handled in GeneralStats
                shouldPathToTarget = false;
                isTouchActive = false;
                shouldSlowOnShot = false;

                agent.speed = 0;
            }
            chaseSound.Pause();
        }
        else if(chaseSound)
        {
            if (agent.velocity.magnitude > 0)
                chaseSound.UnPause();
            else
                chaseSound.Pause();
        }

        base.Update(); // pathing and such
    }

    // Functions

    Transform getFarthestRetreatFromPlayer()
    {
        Transform retreatSpot = transform;
        float farthestDistance = -1;

        foreach (Transform spot in retreatSpots)
        {
            float distance = Vector3.Distance(spot.position, playerTransform.position);

            if (farthestDistance == -1 || distance > farthestDistance)
            {
                retreatSpot = spot;
                farthestDistance = distance;
            }
        }

        return retreatSpot;
    }
}
