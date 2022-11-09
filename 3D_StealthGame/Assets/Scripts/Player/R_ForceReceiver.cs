using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _drag = 0.1f;


    private Vector3 _dampingVelocity;
    private Vector3 _impact;
    private float _verticalVelocity;

    public Vector3 Movement => _impact + Vector3.up * _verticalVelocity;

    
    void Update()
    {
        // ------------GRAVITY-----------------------------------------------------------------------

        // If we are not falling and are standing on the ground...
        if (_verticalVelocity < 0f && _controller.isGrounded)
        {
            _verticalVelocity = Physics.gravity.y * Time.deltaTime;

        }

        else
        {
            // simulates acceleration of falling speed when falling
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;

        }
    }


    // Methods

    public void Jump(float jumpForce)
    {
        _verticalVelocity += jumpForce;

    }
}
