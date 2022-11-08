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
        _animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState)
        {
            case CharacterState.IDLE:
                _animator.SetFloat("SpeedX", Input.GetAxis("Horizontal"));
                _animator.SetFloat("SpeedY", Input.GetAxis("Vertical"));
                //_animator.SetFloat("moveSpeed", _direction.magnitude);
                _animator.SetBool("isJumping", false);
                _animator.SetBool("isGrounded", true);
                break;

        }
    }
}
