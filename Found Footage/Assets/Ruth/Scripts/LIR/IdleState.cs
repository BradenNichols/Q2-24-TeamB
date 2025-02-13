using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : StateMachineBehaviour
{
            //EP 1
    float timer;
      //make sure to have a player tag
    Transform player;
    float chaseRange = 8;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;
        if (timer > 5)
            //"isPatrolling" is the name of a animator layer that = bool
            animator.SetBool("isPatrolling", true); 
        float distance = Vector3.Distance(player.position, animator.transform.position);
        if (distance < chaseRange)
            //"isChasing" is the name of a animator layer that = bool
            animator.SetBool("isChasing", true);

    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
