using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] 

public class PlayerMovement : MonoBehaviour
{
    //public and serialized
    public float _moveSpeed = 10f;
    public float _turnSpeed = 70f;
    public float _jumpForce = 5f;


    //privates and protected
    //private PlayerState _currentState;
    private Vector3 _direction = new Vector3();
    private Rigidbody _rigidbody;
    private Transform _cameraTransform;
    private bool _isJumping = false;


    private void Awake() // usually used for getting components of the object the script is on
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start() // usually used to get components in other objects
    {
        _cameraTransform = Camera.main.transform;
        
    }



    void Update()
    {
        Move();


    }


    private void FixedUpdate()
    {
       // add direction on y-axis to simulate normal gravity when falling, other wise characte will drop only very slowly
       _direction.y = _rigidbody.velocity.y;

        RotateTowardsCamera();
        
        // move character by setting its velocity to the direction of movement calculated earlier
       _rigidbody.velocity = _direction;


    }

    
    //---------------------------M E T H O D S----------------------------------------------------------------------------------------------------------------------
    
    
    private void Move()
    {
        _direction = _cameraTransform.forward * Input.GetAxis("Vertical")   //forward/backward movement: relative to CAMERA
                      + _cameraTransform.right * Input.GetAxis("Horizontal");      //left-right movement: relative to PLAYER

        _direction *= _moveSpeed; // multiply by movement speed to get direction of movement
    }

    private void RotateTowardsCamera()
    {
        
        Vector3 lookDirection = _cameraTransform.forward;   // make a copy of cameraTransform.forward...
        lookDirection.y = 0;    // ... to be able to set the y-axis of the camera to 0
        // --> otherwise player would lean forward when looking down and lean backward when looking up

        Quaternion rotation = Quaternion.LookRotation(lookDirection); // declare the look rotation according to cameraForward...
        rotation = Quaternion.RotateTowards(_rigidbody.rotation, rotation, _turnSpeed * Time.fixedDeltaTime); //create a smoothed rotation value based on the player's turnSpeed
        _rigidbody.MoveRotation(rotation); // ... and use it with the method "MoveRotation" to make the player rotate




    }



}
