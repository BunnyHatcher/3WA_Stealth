using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WALK : CharacterStateBase
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Enter WALK"); 
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get input for movement direction
        _movementDirection = _cameraTransform.forward * Input.GetAxisRaw("Vertical")
                            + _cameraTransform.right * Input.GetAxisRaw("Horizontal");


        _movementDirection.y = 0f;

        // Move into calculated direction
        _controller.Move(_movementDirection.normalized * _playerControlSettings._currentSpeed * Time.deltaTime);

        //Rotation
        Vector3 lookDirection = _cameraTransform.forward;
        lookDirection.y = 0;


        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        _playerTransform.rotation = Quaternion.Lerp(_playerTransform.rotation, lookRotation, _playerControlSettings._turnSpeed * Time.deltaTime);
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
}
