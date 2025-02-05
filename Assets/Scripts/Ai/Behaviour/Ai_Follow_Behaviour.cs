﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Ai_Follow_Behaviour : StateMachineBehaviour
{

    NavMeshAgent agent;
    AiController aiConctroller;
    Transform aiTransform;

    [SerializeField]
    bool CheckInTargetAttackRange;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        aiConctroller = animator.gameObject.GetComponent<AiController>();
        aiTransform = animator.transform;
        agent = animator.gameObject.GetComponent<NavMeshAgent>();

        agent.destination = aiConctroller.Target.transform.position;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (aiConctroller.Target != null)
        {
            agent.destination = aiConctroller.Target.transform.position;

            //MakeSure we still sight the Target
            Ray ray = new Ray(aiTransform.position, aiConctroller.Target.gameObject.transform.position - aiTransform.position);

            Debug.DrawRay(aiTransform.position, aiConctroller.Target.gameObject.transform.position - aiTransform.position, Color.red);

            if (CheckInTargetAttackRange)
            {
                if ( ( agent.destination -agent.transform.position ).magnitude <= aiConctroller.AttackRange )
                {
                    Debug.Log( "inrange" );
                    animator.SetBool( "InAttackRange", true );
                }
            }

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.GetComponent<PlayerController>() == null)
                {
                    aiConctroller.LostTargetActor();
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.destination = animator.transform.position;
    }
}
