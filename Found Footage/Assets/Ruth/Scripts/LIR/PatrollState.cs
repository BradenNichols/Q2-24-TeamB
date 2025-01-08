using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollState : StateMachineBehaviour
{
   // float timer;
   // List<Transform> wayPoints = new List<Transform>();
   // NavMeshAgent agent;
   // override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   // {
   //     agent = animator.GetComponent<NavMeshAgent>();
   //     timer = 0;
   //     //"WayPoints" is the tag 
   //     GameObject go = GameObject.FindObjectWithTag("WayPoints");
   //     foreeach(Transform t in go.transform)
   //         wayPoints.Add(t);
   //
   //     agent.SetDestination(wayPoints[Random.Rage(0, wayPoints.Count)].position);
   // }
   //
   // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   // {
   //     if(agent.remainingDistance <= agent.stoppingDistance)
   //         agent.SetDestination(wayPoints[Random.Rage(0, wayPoints.Count)].position);
   //
   //     timer += Time.deltaTime;
   //     if(timer > 10)
   //         //"isPatrolling" is the name of a animator layer that = bool
   //         animator.SetBool("isPatrolling",false);
   //     //1. Go to 9:26 of the video after this^
   //
   // }
   //
   // public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
   // {
   //     agent.setDestination(agent.transform.position);
   //     //2. 16:00
   // }
}
