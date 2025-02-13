using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollState : StateMachineBehaviour
{
          //EP 1
    float timer;
    List<Transform> wayPoints = new List<Transform>();
    NavMeshAgent agent;

    Transform player;
    float chaseRange = 8;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
        agent.speed = 1.5f;
        timer = 0;
        //"WayPoints" is the tag 
        GameObject go = GameObject.FindGameObjectWithTag("WayPoints");
        foreach(Transform t in go.transform)
            wayPoints.Add(t);
   
        agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
    }
   
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(agent.remainingDistance <= agent.stoppingDistance)
            agent.SetDestination(wayPoints[Random.Range(0, wayPoints.Count)].position);
   
        timer += Time.deltaTime;
        if(timer > 10)
            //"isPatrolling" is the name of a animator layer that = bool
            animator.SetBool("isPatrolling",false);
        //1. Go to 9:26 of the video after this^
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < chaseRange)
            //"isChasing" is the name of a animator layer that = bool
            animator.SetBool("isChasing", true);

    }
   
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(agent.transform.position);
        //2. 16:00
    }
}
