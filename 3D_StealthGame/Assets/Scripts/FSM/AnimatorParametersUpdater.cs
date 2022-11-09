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
    //private IDLE _idleState;


    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        



    }

    // Update is called once per frame
    void Update()
    {
        _direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        _animator.SetFloat("directionMagnitude", _direction.magnitude );
        _animator.SetFloat("moveSpeed", _moveSpeed * 100);
        

        if (Input.GetButton("Run") && _direction.magnitude > 0)
        {
            _animator.SetBool("runningKeyPressed", true);
        }



    }
}
