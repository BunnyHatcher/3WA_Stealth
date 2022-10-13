using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 

public class PlayerMovement : MonoBehaviour
{

    public float _moveSpeed = 10f;

    private Vector3 _direction = new Vector3();
    private Rigidbody _rigidbody;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    void Update()
    {  
        //forward/backward movement                     //left-right movement
        _direction = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        _direction *= _moveSpeed; // multiply by movement speed = direction of movement

    }

    private void FixedUpdate()
    {
       // move character by setting its velocity to the direction of movement calculated earlier
        _rigidbody.velocity = _direction;
    }

}
