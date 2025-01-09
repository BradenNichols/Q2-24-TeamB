using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    //Add NavMesh agent component to the enemy
    NavMeshAgent agent;
    Transform player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent.speed = 3.5f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.position);
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance > 15)
            //"isChasing" is the name of a animator layer that = bool
            animator.SetBool("isChasing", false);
        if (distance < 2.5f)
            animator.SetBool("isAttacking", true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(player.transform.position);
    }
}
