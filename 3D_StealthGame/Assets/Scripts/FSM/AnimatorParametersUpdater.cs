using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterState
{
    IDLE,
    WALKING,
    JOGGING,
    RUNNING,
    SNEAKING,
    JUMPING,
    FALLING,
    DODGING,

}

public class AnimatorParametersUpdater : MonoBehaviour
{
    /// <summary>
    /// Script managing all animator parameters
    /// like SetFloat, SetBool, SetTrigger etc.
    /// </summary>
    /// 


    private CharacterState _currentState;
    private Animator _animator;


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        

    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case CharacterState.IDLE:
                _animator.SetFloat("SpeedX", Input.GetAxis("Horizontal"));
                _animator.SetFloat("SpeedY", Input.GetAxis("Vertical"));
                //_animator.SetFloat("moveSpeed", _movementDirection.magnitude);
                _animator.SetBool("isJumping", false);
                _animator.SetBool("isGrounded", true);
                break;

            case CharacterState.WALKING:
                break;

            case CharacterState.JOGGING:
                _animator.SetFloat("SpeedX", Input.GetAxis("Horizontal"));
                _animator.SetFloat("SpeedY", Input.GetAxis("Vertical"));
                //_animator.SetFloat("moveSpeed", _movementDirection.magnitude);
                _animator.SetBool("isJumping", false);
                _animator.SetBool("isGrounded", true);

                break;

            case CharacterState.RUNNING:
                break;

            case CharacterState.SNEAKING:
                break;

            case CharacterState.JUMPING:
                _animator.SetBool("isJumping", true);
                _animator.SetBool("isGrounded", false);
                break;

            case CharacterState.FALLING:
                _animator.SetBool("isJumping", false);
                _animator.SetBool("isGrounded", false);
                break;




        }
    }
}
