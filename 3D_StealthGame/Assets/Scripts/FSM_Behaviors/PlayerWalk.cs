using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : CharacterStateBase
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get input for movement direction
        GetPlayerControls(animator)._movementDirection =
        GetPlayerControls(animator)._cameraTransform.forward * Input.GetAxisRaw("Vertical")
       + GetPlayerControls(animator)._cameraTransform.right * Input.GetAxisRaw("Horizontal");


        GetPlayerControls(animator)._movementDirection.y = 0f;

        // Move into calculated direction
        GetPlayerControls(animator)._controller.Move(GetPlayerControls(animator)._movementDirection.normalized
        * GetPlayerControls(animator)._currentSpeed * Time.deltaTime);

        //Rotation
        Vector3 lookDirection = GetPlayerControls(animator)._cameraTransform.forward;
        lookDirection.y = 0;


        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        GetPlayerControls(animator).transform.rotation
        = Quaternion.Lerp(GetPlayerControls(animator).transform.rotation,
        lookRotation, GetPlayerControls(animator)._turnSpeed * Time.fixedDeltaTime);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    
}
