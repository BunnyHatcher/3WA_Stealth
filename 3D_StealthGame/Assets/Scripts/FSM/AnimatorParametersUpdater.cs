using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class AnimatorParametersUpdater : MonoBehaviour
{
    /// <summary>
    /// Singleton script managing all animator parameters
    /// like SetFloat, SetBool, SetTrigger etc.
    /// </summary>
    /// 


    public Vector2 _direction; //Joystick direction, not to confuse with player speed, velocity etc.
    public float _moveSpeed;
    
    private Animator _animator;
   


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("isJumping", false);
        _animator.SetBool("isGrounded", true);



    }

    // Update is called once per frame
    void Update()
    {
        // Referencing necessary parameters
        _direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // IDLE
        if (_direction.magnitude == 0)
        {
            _animator.SetFloat("directionMagnitude", _direction.magnitude);
            _animator.SetFloat("moveSpeed", _moveSpeed * 100);
            _animator.SetBool("isJumping", false);
            _animator.SetBool("isGrounded", true);
        }
        
        

        // JOGGING
        if (_direction.magnitude > 0)
        {
            _animator.SetFloat("directionMagnitude", _direction.magnitude);
            _animator.SetFloat("moveSpeed", _moveSpeed * 100);
            _animator.SetFloat("SpeedX", Input.GetAxis("Horizontal"));
            _animator.SetFloat("SpeedY", Input.GetAxis("Vertical"));
        }

        // RUN
        if (Input.GetButton("Run") && _direction.magnitude > 0)
        {
            _animator.SetBool("runningKeyPressed", true);
        }
        
        // JUMP




    }
}
