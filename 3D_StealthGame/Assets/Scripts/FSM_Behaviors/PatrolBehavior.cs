using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class PatrolBehavior : StateMachineBehaviour
{

    private NavMeshAgent _agent;
    
    //Patrol Behaviour
    public Transform[] _points;
    private int _destPoint = 0;
    private bool _goingForward = true;
    [SerializeField] private bool _backAndForth = false;




    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent = animator.GetComponent<NavMeshAgent>();
        GameObject gameObject = animator.gameObject;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PatrolMovement();
    }


    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
    
    




    // Methods

    private void PatrolMovement()
    {
        if (_backAndForth)
        {
            if (_goingForward)
            { GotoNextPoint(); }

            else
            { GoToPreviousPoint(); }
        }

        else
        {

            GotoNextPoint();

        }
    }


    private void GotoNextPoint()
    {
        // Choose the next destination point when the agent gets
        // close to the current one.
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)


        { // Returns if no points have been set up
            if (_points.Length == 0)
                return;

            _destPoint++;

            if (_destPoint >= _points.Length)
            {
                if (_backAndForth)
                {
                    _goingForward = false;
                    _destPoint = _points.Length - 1;
                }

                else
                {
                    _destPoint = 0;
                }
            }

            // Set the agent to go to the currently selected destination.
            _agent.destination = _points[_destPoint].position;

        }
    }

    private void GoToPreviousPoint()
    {
        if (!_agent.pathPending && _agent.remainingDistance < 0.5f)
        {
            _destPoint--;

            if (_destPoint < 0)
            {
                if (_backAndForth)
                {
                    _goingForward = true;
                    _destPoint = 0;
                }

                else
                {
                    _destPoint = _points.Length - 1;
                }
            }

            _agent.destination = _points[_destPoint].position;

        }


    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < _points.Length - 1; i++)
        {
            if (i == _points.Length) // at the last waypoint
            {
                Gizmos.DrawLine(_points[i].position, _points[0].position);

            }

            else
            {
                Gizmos.DrawLine(_points[i].position, _points[i + 1].position);
            }

        }
    }





}
