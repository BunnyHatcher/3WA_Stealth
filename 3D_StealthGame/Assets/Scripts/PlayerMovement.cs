using System;
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

    [Header("FloorDetection")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Vector3 _boxDimension;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float yFloorOffset = 1f;
    private FloorDetector _floorDetector;



    //privates and protected
    private Vector3 _direction = new Vector3();
    private Rigidbody _rigidbody;
    private Transform _cameraTransform;
    private Animator _animator;
    private bool _isJumping = false;
    private bool _isGrounded = true;


    private void Awake() // usually used for getting components of the object the script is on
    {
        _rigidbody = GetComponent<Rigidbody>();
        _floorDetector = GetComponentInChildren<FloorDetector>();
    }

    private void Start() // usually used to get components in other objects
    {
        _cameraTransform = Camera.main.transform;
        
        
        
    }



    void Update()
    {
        Move();

        Jump();

        // Draw an boxes that represents the ground checker
        Collider[] groundColliders = Physics.OverlapBox(_groundChecker.position, _boxDimension, Quaternion.identity, _groundMask);

        _isGrounded = groundColliders.Length > 0;// if more than one ground collider touches the ground, isGrounded becomes true

        if ( _isGrounded ) 
        {
            //Debug.Log("Touchdown!");
        }

        

        



    }


    private void FixedUpdate()
    {
       if (_isJumping )
        {
            _direction.y = _jumpForce;
            _isJumping = false; // to prevent jump is automatically prolonged every frame after the first one; otherwise player will rise into air like a hot baloon
        }

       else
        { 
         // add direction on y-axis to simulate normal gravity when falling, other wise characte will drop only very slowly
       _direction.y = _rigidbody.velocity.y;
        }

        StickToGround();

        RotateTowardsCamera();
        
        // move character by setting its velocity to the direction of movement calculated earlier
       _rigidbody.velocity = _direction;

    }


 

    //---------------------------| O T H E R  M E T H O D S | ----------------------------------------------------------------------------------------------------------------------

    private void Move()
    {
        _direction = _cameraTransform.forward * Input.GetAxis("Vertical")   //forward/backward movement: relative to CAMERA
                      + _cameraTransform.right * Input.GetAxis("Horizontal");      //left-right movement: relative to PLAYER

        _direction *= _moveSpeed; // multiply by movement speed to get direction of movement

        _direction.y = 0; // Vertical transform is not taken into account, we have the Jump method for that
    }

    private void StickToGround()
    {
        Vector3 averagePosition = _floorDetector.AverageHeight();

        Vector3 newPosition = new Vector3(_rigidbody.position.x, averagePosition.y + yFloorOffset, _rigidbody.position.z); // glues the character to the average position on the y-axis
        _rigidbody.MovePosition( newPosition);
        _direction.y = 0;
    }

    private void RotateTowardsCamera()
    {

        Vector3 lookDirection = _cameraTransform.forward;   // make a copy of cameraTransform.forward...
        lookDirection.y = 0;    // ... fix look direction on y-axis
        // --> otherwise player would lean forward when looking down and lean backward when looking up

        Quaternion rotation = Quaternion.LookRotation(lookDirection); // declare the look rotation according to look direction previously calculated...
        rotation = Quaternion.RotateTowards(_rigidbody.rotation, rotation, _turnSpeed * Time.fixedDeltaTime); //create a smoothed rotation value based on the player's turnSpeed
        _rigidbody.MoveRotation(rotation); // ... and use it with the method "MoveRotation" to make the player rotate

    }

    private void Jump()
    {
        if(Input.GetButtonDown("Jump") && _isGrounded == true)
        {
            _isJumping = true;
           // _animator.SetBool("isJumping", true);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(_groundChecker.position, _boxDimension * 2f); // by default OverlapBox only takes half the size of the box in each dimension, so we need to double the size of _boxDimension
    }
    

}
